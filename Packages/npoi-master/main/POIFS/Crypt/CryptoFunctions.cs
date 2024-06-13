/* ====================================================================
   Licensed to the Apache Software Foundation (ASF) under one or more
   contributor license agreements.  See the NOTICE file distributed with
   this work for Additional information regarding copyright ownership.
   The ASF licenses this file to You under the Apache License, Version 2.0
   (the "License"); you may not use this file except in compliance with
   the License.  You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
==================================================================== */
namespace NPOI.POIFS.Crypt
{
    using NPOI.Util;
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Security;
    using System;
    using System.Text;

    /**
     * Helper functions used for standard and agile encryption
     */
    public class CryptoFunctions {

        //arbitrarily selected; may need to increase
        private const int DEFAULT_MAX_RECORD_LENGTH = 100_000;
        public const int MAX_RECORD_LENGTH = DEFAULT_MAX_RECORD_LENGTH;

        /**
         * <p><cite>2.3.4.7 ECMA-376 Document Encryption Key Generation (Standard Encryption)<br/>
         * 2.3.4.11 Encryption Key Generation (Agile Encryption)</cite></p>
         * 
         * <p>The encryption key for ECMA-376 document encryption [ECMA-376] using agile
         * encryption MUST be generated by using the following method, which is derived from PKCS #5:
         * <a href="https://www.ietf.org/rfc/rfc2898.txt">Password-Based Cryptography Version 2.0 [RFC2898]</a>.</p>
         * 
         * <p>Let H() be a hashing algorithm as determined by the PasswordKeyEncryptor.hashAlgorithm
         * element, H_n be the hash data of the n-th iteration, and a plus sign (+) represent concatenation.
         * The password MUST be provided as an array of Unicode characters. Limitations on the length of the
         * password and the characters used by the password are implementation-dependent.
         * The initial password hash is generated as follows:</p>
         * 
         * 
         * <pre>H_0 = H(salt + password)</pre>
         * 
         * <p>The salt used MUST be generated randomly. The salt MUST be stored in the
         * PasswordKeyEncryptor.saltValue element contained within the \EncryptionInfo stream as
         * specified in section 2.3.4.10. The hash is then iterated by using the following approach:</p>
         * 
         * <pre>H_n = H(iterator + H_n-1)</pre>
         * 
         * <p>where iterator is an unsigned 32-bit value that is initially set to 0x00000000 and then incremented
         * monotonically on each iteration until PasswordKey.spinCount iterations have been performed.
         * The value of iterator on the last iteration MUST be one less than PasswordKey.spinCount.</p>
         * 
         * <p>For POI, H_final will be calculated by {@link #generateKey(byte[],HashAlgorithm,byte[],int)}</p>
         *
         * @param password
         * @param hashAlgorithm
         * @param salt
         * @param spinCount
         * @return the hashed password
         */
        public static byte[] HashPassword(String password, HashAlgorithm hashAlgorithm, byte[] salt, int spinCount) {
            return HashPassword(password, hashAlgorithm, salt, spinCount, true);
        }

        /**
         * Generalized method for read and write protection hash generation.
         * The difference is, read protection uses the order iterator then hash in the hash loop, whereas write protection
         * uses first the last hash value and then the current iterator value
         *
         * @param password
         * @param hashAlgorithm
         * @param salt
         * @param spinCount
         * @param iteratorFirst if true, the iterator is hashed before the n-1 hash value,
         *        if false the n-1 hash value is applied first
         * @return the hashed password
         */
        public static byte[] HashPassword(String password, HashAlgorithm hashAlgorithm, byte[] salt, int spinCount, bool iteratorFirst) {
            // If no password was given, use the default
            if (password == null) {
                password = Decryptor.DEFAULT_PASSWORD;
            }

            MessageDigest hashAlg = GetMessageDigest(hashAlgorithm);

            hashAlg.Update(salt);
            byte[] hash = hashAlg.Digest(StringUtil.GetToUnicodeLE(password));
            byte[] iterator = new byte[LittleEndianConsts.INT_SIZE];

            byte[] first = (iteratorFirst ? iterator : hash);
            byte[] second = (iteratorFirst ? hash : iterator);

            try {
                for (int i = 0; i < spinCount; i++) {
                    LittleEndian.PutInt(iterator, 0, i);
                    hashAlg.Reset();
                    hashAlg.Update(first);
                    hashAlg.Update(second);
                    hashAlg.Digest(hash, 0, hash.Length); // don't create hash buffer everytime new
                }
            } catch (Exception e) {
                throw new EncryptedDocumentException("error in password hashing", e);
            }

            return hash;
        }

        /**
        * <p><cite>2.3.4.12 Initialization Vector Generation (Agile Encryption)</cite></p>
        * 
        * <p>Initialization vectors are used in all cases for agile encryption. An initialization vector MUST be
        * generated by using the following method, where H() is a hash function that MUST be the same as
        * specified in section 2.3.4.11 and a plus sign (+) represents concatenation:</p>
        * <ul>
        * <li>If a blockKey is provided, let IV be a hash of the KeySalt and the following value:<br/>
        *     {@code blockKey: IV = H(KeySalt + blockKey)}</li>
        * <li>If a blockKey is not provided, let IV be equal to the following value:<br/>
        *     {@code KeySalt:IV = KeySalt}</li>
        * <li>If the number of bytes in the value of IV is less than the the value of the blockSize attribute
        *     corresponding to the cipherAlgorithm attribute, pad the array of bytes by appending 0x36 until
        *     the array is blockSize bytes. If the array of bytes is larger than blockSize bytes, truncate the
        *     array to blockSize bytes.</li>
        * </ul> 
        **/
        public static byte[] GenerateIv(HashAlgorithm hashAlgorithm, byte[] salt, byte[] blockKey, int blockSize) {
            byte[] iv = salt;
            if (blockKey != null) {
                MessageDigest hashAlgo = GetMessageDigest(hashAlgorithm);
                hashAlgo.Update(salt);
                iv = hashAlgo.Digest(blockKey);
            }
            return GetBlock36(iv, blockSize);
        }

        /**
         * <p><cite>2.3.4.11 Encryption Key Generation (Agile Encryption)</cite></p>
         * 
         * <p>The final hash data that is used for an encryption key is then generated by using the following
         * method:</p>
         * 
         * <pre>H_final = H(H_n + blockKey)</pre>
         * 
         * <p>where blockKey represents an array of bytes used to prevent two different blocks from encrypting
         * to the same cipher text.</p>
         * 
         * <p>If the size of the resulting H_final is smaller than that of PasswordKeyEncryptor.keyBits, the key
         * MUST be padded by appending bytes with a value of 0x36. If the hash value is larger in size than
         * PasswordKeyEncryptor.keyBits, the key is obtained by truncating the hash value.</p> 
         *
         * @param passwordHash
         * @param hashAlgorithm
         * @param blockKey
         * @param keySize
         * @return intermediate key
         */
        public static byte[] GenerateKey(byte[] passwordHash, HashAlgorithm hashAlgorithm, byte[] blockKey, int keySize) {
            MessageDigest hashAlgo = GetMessageDigest(hashAlgorithm);
            hashAlgo.Update(passwordHash);
            byte[] key = hashAlgo.Digest(blockKey);
            return GetBlock36(key, keySize);
        }

        /**
        * Initialize a new cipher object with the given cipher properties and no padding
        * If the given algorithm is not implemented in the JCE, it will try to load it from the bouncy castle
        * provider.
        *
        * @param key the secrect key
        * @param cipherAlgorithm the cipher algorithm
        * @param chain the chaining mode
        * @param vec the initialization vector (IV), can be null
        * @param cipherMode Cipher.DECRYPT_MODE or Cipher.ENCRYPT_MODE
        * @return the requested cipher
        * @throws GeneralSecurityException
        * @throws EncryptedDocumentException if the initialization failed or if an algorithm was specified,
        *   which depends on a missing bouncy castle provider 
        */
        public static Cipher GetCipher(ISecretKey key, CipherAlgorithm cipherAlgorithm, ChainingMode chain, byte[] vec, int cipherMode) {
            return GetCipher(key, cipherAlgorithm, chain, vec, cipherMode, null);
        }

        /**
         * Initialize a new cipher object with the given cipher properties
         * If the given algorithm is not implemented in the JCE, it will try to load it from the bouncy castle
         * provider.
         *
         * @param key the secrect key
         * @param cipherAlgorithm the cipher algorithm
         * @param chain the chaining mode
         * @param vec the Initialization vector (IV), can be null
         * @param cipherMode Cipher.DECRYPT_MODE or Cipher.ENCRYPT_MODE
         * @param padding the padding (null = NOPADDING, ANSIX923Padding, PKCS5Padding, PKCS7Padding, ISO10126Padding, ...)
         * @return the requested cipher
         * @throws GeneralSecurityException
         * @throws EncryptedDocumentException if the Initialization failed or if an algorithm was specified,
         *   which depends on a missing bouncy castle provider 
         */
        public static Cipher GetCipher(IKey key, CipherAlgorithm cipherAlgorithm, ChainingMode chain, byte[] vec, int cipherMode, String padding) {
            int keySizeInBytes = key.GetEncoded().Length;
            if (padding == null) padding = "NoPadding";

            try {
                // Ensure the JCE policies files allow for this sized key
                /*if (Cipher.GetMaxAllowedKeyLength(cipherAlgorithm.jceId) < keySizeInBytes * 8) {
                    throw new EncryptedDocumentException("Export Restrictions in place - please install JCE Unlimited Strength Jurisdiction Policy files");
                }*/

                Cipher cipher;
                if (cipherAlgorithm == CipherAlgorithm.rc4) {
                    cipher = Cipher.GetInstance(cipherAlgorithm.jceId);
                } else if (cipherAlgorithm.needsBouncyCastle) {
                    registerBouncyCastle();
                    cipher = Cipher.GetInstance(cipherAlgorithm.jceId + "/" + chain.jceId + "/" + padding, "BC");
                } else {
                    cipher = Cipher.GetInstance(cipherAlgorithm.jceId + "/" + chain.jceId + "/" + padding);
                }

                if (vec == null) {
                    cipher.Init(cipherMode, key);
                } else {
                    AlgorithmParameterSpec aps;
                    if (cipherAlgorithm == CipherAlgorithm.rc2) {
                        aps = new RC2ParameterSpec(key.GetEncoded().Length * 8, vec);
                    } else {
                        aps = new IvParameterSpec(vec);
                    }
                    cipher.Init(cipherMode, key, aps);
                }
                return cipher;
            } catch (Exception e) {
                throw new EncryptedDocumentException(e);
            }
        }

        /**
         * Returns a new byte array with a tRuncated to the given size. 
         * If the hash has less then size bytes, it will be Filled with 0x36-bytes
         *
         * @param hash the to be tRuncated/filled hash byte array
         * @param size the size of the returned byte array
         * @return the pAdded hash
         */
        private static byte[] GetBlock36(byte[] hash, int size) {
            return GetBlockX(hash, size, (byte)0x36);
        }

        /**
         * Returns a new byte array with a tRuncated to the given size. 
         * If the hash has less then size bytes, it will be Filled with 0-bytes
         *
         * @param hash the to be tRuncated/filled hash byte array
         * @param size the size of the returned byte array
         * @return the pAdded hash
         */
        public static byte[] GetBlock0(byte[] hash, int size) {
            return GetBlockX(hash, size, (byte)0);
        }

        private static byte[] GetBlockX(byte[] hash, int size, byte Fill) {
            if (hash.Length == size) return hash;

            byte[] result = new byte[size];
            Arrays.Fill(result, Fill);
            Array.Copy(hash, 0, result, 0, Math.Min(result.Length, hash.Length));
            return result;
        }

        public static MessageDigest GetMessageDigest(HashAlgorithm hashAlgorithm) {
            try {
                if (hashAlgorithm.needsBouncyCastle) {
                    registerBouncyCastle();
                    return MessageDigest.GetInstance(hashAlgorithm.jceId, "BC");
                } else {
                    return MessageDigest.GetInstance(hashAlgorithm.jceId);
                }
            } catch (Exception e) {
                throw new EncryptedDocumentException("hash algo not supported", e);
            }
        }

        public static Mac GetMac(HashAlgorithm hashAlgorithm) {
            try {
                if (hashAlgorithm.needsBouncyCastle) {
                    registerBouncyCastle();
                    return Mac.GetInstance(hashAlgorithm.jceHmacId, "BC");
                } else {
                    return Mac.GetInstance(hashAlgorithm.jceHmacId);
                }
            } catch (Exception e) {
                throw new EncryptedDocumentException("hmac algo not supported", e);
            }
        }

        [Obsolete("not necessary for npoi")]
        public static void registerBouncyCastle() {
            //if (Security.GetProvider("BC") != null) return;
            //try {
            //    ClassLoader cl = Thread.CurrentThread().ContextClassLoader;
            //    String bcProviderName = "org.bouncycastle.jce.provider.BouncyCastleProvider";
            //    Class<Provider> clazz = (Class<Provider>)cl.LoadClass(bcProviderName);
            //    Security.AddProvider(clazz.NewInstance());
            //} catch (Exception e) {
            //    throw new EncryptedDocumentException("Only the BouncyCastle provider supports your encryption Settings - please add it to the classpath.");
            //}
        }

        private static int[] INITIAL_CODE_ARRAY = {
            0xE1F0, 0x1D0F, 0xCC9C, 0x84C0, 0x110C, 0x0E10, 0xF1CE,
            0x313E, 0x1872, 0xE139, 0xD40F, 0x84F9, 0x280C, 0xA96A,
            0x4EC3
        };

        private static byte[] PAD_ARRAY = {
            (byte)0xBB, (byte)0xFF, (byte)0xFF, (byte)0xBA, (byte)0xFF,
            (byte)0xFF, (byte)0xB9, (byte)0x80, (byte)0x00, (byte)0xBE,
            (byte)0x0F, (byte)0x00, (byte)0xBF, (byte)0x0F, (byte)0x00
        };

        private static int[][] ENCRYPTION_MATRIX = {
            /* char 1  */ new int[] {0xAEFC, 0x4DD9, 0x9BB2, 0x2745, 0x4E8A, 0x9D14, 0x2A09},
            /* char 2  */ new int[] {0x7B61, 0xF6C2, 0xFDA5, 0xEB6B, 0xC6F7, 0x9DCF, 0x2BBF},
            /* char 3  */ new int[] {0x4563, 0x8AC6, 0x05AD, 0x0B5A, 0x16B4, 0x2D68, 0x5AD0},
            /* char 4  */ new int[] {0x0375, 0x06EA, 0x0DD4, 0x1BA8, 0x3750, 0x6EA0, 0xDD40},
            /* char 5  */ new int[] {0xD849, 0xA0B3, 0x5147, 0xA28E, 0x553D, 0xAA7A, 0x44D5},
            /* char 6  */ new int[] {0x6F45, 0xDE8A, 0xAD35, 0x4A4B, 0x9496, 0x390D, 0x721A},
            /* char 7  */ new int[] {0xEB23, 0xC667, 0x9CEF, 0x29FF, 0x53FE, 0xA7FC, 0x5FD9},
            /* char 8  */ new int[] {0x47D3, 0x8FA6, 0x0F6D, 0x1EDA, 0x3DB4, 0x7B68, 0xF6D0},
            /* char 9  */ new int[] {0xB861, 0x60E3, 0xC1C6, 0x93AD, 0x377B, 0x6EF6, 0xDDEC},
            /* char 10 */ new int[] {0x45A0, 0x8B40, 0x06A1, 0x0D42, 0x1A84, 0x3508, 0x6A10},
            /* char 11 */ new int[] {0xAA51, 0x4483, 0x8906, 0x022D, 0x045A, 0x08B4, 0x1168},
            /* char 12 */ new int[] {0x76B4, 0xED68, 0xCAF1, 0x85C3, 0x1BA7, 0x374E, 0x6E9C},
            /* char 13 */ new int[] {0x3730, 0x6E60, 0xDCC0, 0xA9A1, 0x4363, 0x86C6, 0x1DAD},
            /* char 14 */ new int[] {0x3331, 0x6662, 0xCCC4, 0x89A9, 0x0373, 0x06E6, 0x0DCC},
            /* char 15 */ new int[] {0x1021, 0x2042, 0x4084, 0x8108, 0x1231, 0x2462, 0x48C4}
        };

        /**
         * Create the verifier for xor obfuscation (method 1)
         *
         * @see <a href="http://msdn.microsoft.com/en-us/library/dd926947.aspx">2.3.7.1 Binary Document Password Verifier Derivation Method 1</a>
         * @see <a href="http://msdn.microsoft.com/en-us/library/dd905229.aspx">2.3.7.4 Binary Document Password Verifier Derivation Method 2</a>
         * @see <a href="http://www.ecma-international.org/news/TC45_current_work/Office Open XML Part 4 - Markup Language Reference.pdf">Part 4 - Markup Language Reference - Ecma International - 3.2.12 fileSharing</a>
         * 
         * @param password the password
         * @return the verifier (actually a short value)
         */
        public static int CreateXorVerifier1(String password)
        {
            byte[] arrByteChars = toAnsiPassword(password);

            // SET Verifier TO 0x0000
            short verifier = 0;
            if (!"".Equals(password))
            {
                // FOR EACH PasswordByte IN PasswordArray IN REVERSE ORDER
                for (int i = arrByteChars.Length - 1; i >= 0; i--)
                {
                    // SET Verifier TO Intermediate3 BITWISE XOR PasswordByte
                    verifier = rotateLeftBase15Bit(verifier);
                    verifier ^= arrByteChars[i];
                }

                // as we haven't prepended the password length into the input array
                // we need to do it now separately ...
                verifier = rotateLeftBase15Bit(verifier);
                verifier ^= (short)arrByteChars.Length;

                // RETURN Verifier BITWISE XOR 0xCE4B
                verifier ^= unchecked((short)0xCE4B); // (0x8000 | ('N' << 8) | 'K')
            }
            return verifier & 0xFFFF;
        }

        /**
         * This method generates the xor verifier for word documents &lt; 2007 (method 2).
         * Its output will be used as password input for the newer word generations which
         * utilize a real hashing algorithm like sha1.
         * 
         * @param password the password
         * @return the hashed password
         * 
         * @see <a href="http://msdn.microsoft.com/en-us/library/dd905229.aspx">2.3.7.4 Binary Document Password Verifier Derivation Method 2</a>
         * @see <a href="http://blogs.msdn.com/b/vsod/archive/2010/04/05/how-to-set-the-editing-restrictions-in-word-using-open-xml-sdk-2-0.aspx">How to Set the editing restrictions in Word using Open XML SDK 2.0</a>
         * @see <a href="http://www.aspose.com/blogs/aspose-blogs/vladimir-averkin/archive/2007/08/20/funny-how-the-new-powerful-cryptography-implemented-in-word-2007-turns-it-into-a-perfect-tool-for-document-password-removal.html">Funny: How the new powerful cryptography implemented in Word 2007 turns it into a perfect tool for document password removal.</a>
         */
        public static int CreateXorVerifier2(String password) {
            //Array to hold Key Values
            byte[] generatedKey = new byte[4];

            //Maximum length of the password is 15 chars.
            int maxPasswordLength = 15;

            if (!"".Equals(password)) {
                // TRuncate the password to 15 characters
                password = password.Substring(0, Math.Min(password.Length, maxPasswordLength));

                byte[] arrByteChars = toAnsiPassword(password);

                // Compute the high-order word of the new key:

                // --> Initialize from the Initial code array (see below), depending on the passwords length. 
                int highOrderWord = INITIAL_CODE_ARRAY[arrByteChars.Length - 1];

                // --> For each character in the password:
                //      --> For every bit in the character, starting with the least significant and progressing to (but excluding) 
                //          the most significant, if the bit is Set, XOR the keys high-order word with the corresponding word from 
                //          the Encryption Matrix
                for (int i = 0; i < arrByteChars.Length; i++) {
                    int tmp = maxPasswordLength - arrByteChars.Length + i;
                    for (int intBit = 0; intBit < 7; intBit++) {
                        if ((arrByteChars[i] & (0x0001 << intBit)) != 0) {
                            highOrderWord ^= ENCRYPTION_MATRIX[tmp][intBit];
                        }
                    }
                }

                // Compute the low-order word of the new key:
                int verifier = CreateXorVerifier1(password);


                // The byte order of the result shall be reversed [password "Example": 0x64CEED7E becomes 7EEDCE64],
                // and that value shall be hashed as defined by the attribute values.

                LittleEndian.PutShort(generatedKey, 0, (short)verifier);
                LittleEndian.PutShort(generatedKey, 2, (short)highOrderWord);
            }

            return LittleEndian.GetInt(generatedKey);
        }

        /**
         * This method generates the xored-hashed password for word documents &lt; 2007.
         */
        public static String XorHashPassword(String password) {
            int hashedPassword = CreateXorVerifier2(password);
            return String.Format("{0:X8}", hashedPassword);
        }

        /**
         * Convenience function which returns the reversed xored-hashed password for further 
         * Processing in word documents 2007 and newer, which utilize a real hashing algorithm like sha1.
         */
        public static String XorHashPasswordReversed(String password) {
            int hashedPassword = CreateXorVerifier2(password);

            return String.Format("{0:X2}{1:X2}{2:X2}{3:X2}"
                , Operator.UnsignedRightShift(hashedPassword , 0) & 0xFF
                , Operator.UnsignedRightShift(hashedPassword , 8) & 0xFF
                , Operator.UnsignedRightShift(hashedPassword , 16) & 0xFF
                , Operator.UnsignedRightShift(hashedPassword , 24) & 0xFF
            );
        }


        /**
         * Create the xor key for xor obfuscation, which is used to create the xor array (method 1)
         *
         * @see <a href="http://msdn.microsoft.com/en-us/library/dd924704.aspx">2.3.7.2 Binary Document XOR Array Initialization Method 1</a>
         * @see <a href="http://msdn.microsoft.com/en-us/library/dd905229.aspx">2.3.7.4 Binary Document Password Verifier Derivation Method 2</a>
         * 
         * @param password the password
         * @return the xor key
         */
        public static int CreateXorKey1(string password) {
            // the xor key for method 1 is part of the verifier for method 2
            // so we simply chop it from there
            //return CreateXorVerifier2(password) >>> 16;
            return Operator.UnsignedRightShift(CreateXorVerifier2(password), 16);
        }

        /**
         * Creates an byte array for xor obfuscation (method 1) 
         *
         * @see <a href="http://msdn.microsoft.com/en-us/library/dd924704.aspx">2.3.7.2 Binary Document XOR Array Initialization Method 1</a>
         * @see <a href="http://docs.libreoffice.org/oox/html/binarycodec_8cxx_source.html">Libre Office implementation</a>
         *
         * @param password the password
         * @return the byte array for xor obfuscation
         */
        public static byte[] CreateXorArray1(string password) {
            if (password.Length > 15) password = password.Substring(0, 15);
            byte[] passBytes = Encoding.ASCII.GetBytes(password); // password.GetBytes(Charset.ForName("ASCII"));

            // this code is based on the libre office implementation.
            // The MS-OFFCRYPTO misses some infos about the various rotation sizes 
            byte[] obfuscationArray = new byte[16];
            Array.Copy(passBytes, 0, obfuscationArray, 0, passBytes.Length);
            Array.Copy(PAD_ARRAY, 0, obfuscationArray, passBytes.Length, PAD_ARRAY.Length - passBytes.Length + 1);

            int xorKey = CreateXorKey1(password);

            // rotation of key values is application dependent /* Excel = 2; Word = 7 */
            int nRotateSize = 2; 
            int op = Operator.UnsignedRightShift(xorKey, 8); //op => (xorKey >>> 8)
            byte[] baseKeyLE = { (byte)(xorKey & 0xFF), (byte)(op & 0xFF) };
            
            for (int i = 0; i < obfuscationArray.Length; i++) {
                obfuscationArray[i] ^= baseKeyLE[i & 1];
                obfuscationArray[i] = rotateLeft(obfuscationArray[i], nRotateSize);
            }

            return obfuscationArray;
        }

        /**
         * The provided Unicode password string is converted to a ANSI string
         *
         * @param password the password
         * @return the ansi bytes
         * 
         * @see <a href="http://www.ecma-international.org/news/TC45_current_work/Office Open XML Part 4 - Markup Language Reference.pdf">Part 4 - Markup Language Reference - Ecma International</a> (3.2.29 workbookProtection)
         */
        private static byte[] toAnsiPassword(String password)
        {
            // TODO: charset conversion (see ecma spec) 

            // Get the single-byte values by iterating through the Unicode characters.
            // For each character, if the low byte is not equal to 0, take it.
            // Otherwise, take the high byte.
            byte[] arrByteChars = new byte[password.Length];

            for (int i = 0; i < password.Length; i++)
            {
                int intTemp = password[i];
                byte lowByte = (byte)(intTemp & 0xFF);
                //byte highByte = (byte)((intTemp >>> 8) & 0xFF);
                byte highByte = (byte)(Operator.UnsignedRightShift(intTemp , 8) & 0xFF);
                arrByteChars[i] = (lowByte != 0 ? lowByte : highByte);
            }

            return arrByteChars;
        }

        private static byte rotateLeft(byte bits, int Shift) {
            //return (byte)(((bits & 0xff) << Shift) | ((bits & 0xff) >>> (8 - Shift)));
            return (byte)(((bits & 0xff) << Shift) | Operator.UnsignedRightShift((bits & 0xff) , (8 - Shift)));
        }

        private static short rotateLeftBase15Bit(short verifier) {
            /*
             * IF (Verifier BITWISE AND 0x4000) is 0x0000
             *    SET Intermediate1 TO 0
             * ELSE
             *    SET Intermediate1 TO 1
             * ENDIF
             */
            short intermediate1 = (short)(((verifier & 0x4000) == 0) ? 0 : 1);
            /*
             *  SET Intermediate2 TO Verifier MULTIPLED BY 2
             *  SET most significant bit of Intermediate2 TO 0
             */
            short intermediate2 = (short)((verifier << 1) & 0x7FFF);
            /*
             *  SET Intermediate3 TO Intermediate1 BITWISE OR Intermediate2
             */
            short intermediate3 = (short)(intermediate1 | intermediate2);
            return intermediate3;
        }

        public class Mac
        {
            internal static Mac GetInstance(string jceHmacId, string v)
            {
                throw new NotImplementedException();
            }

            internal static Mac GetInstance(string jceHmacId)
            {
                throw new NotImplementedException();
            }

            public byte[] DoFinal(object encoded)
            {
                throw new NotImplementedException();
            }

            public byte[] DoFinal()
            {
                throw new NotImplementedException();
            }

            public void Init(ISecretKey secretKey)
            {
                throw new NotImplementedException();
            }

            public void Update(byte[] buf, int v, int readBytes)
            {
                throw new NotImplementedException();
            }
        }
    }
    public interface IKeySpec { }
    public class SecretKeySpec : IKeySpec, ISecretKey
    {
        private byte[] key;
        private string algorithm;

        public SecretKeySpec(byte[] key, string algorithm)
        {
            if ((key == null) || (algorithm == null))
            {
                throw new ArgumentException("Missing argument");
            }
            if (key.Length == 0)
            {
                throw new ArgumentException("Empty key");
            }
            this.key = new byte[key.Length];
            Array.Copy(key, this.key, key.Length);
            this.algorithm = algorithm;
        }

        public string GetAlgorithm()
        {
            return algorithm;
        }

        public byte[] GetEncoded()
        {
            byte[] ret = new byte[key.Length];
            Array.Copy(key, ret, key.Length);
            return ret;
        }

        public string GetFormat()
        {
            return "RAW";
        }

    }
    public class RC2ParameterSpec : AlgorithmParameterSpec
    {
        private byte[] iv = null;
        private int effectiveKeyBits;

        public RC2ParameterSpec(int v, byte[] vec)
        {
            this.effectiveKeyBits = v;
            this.iv = vec;
        }
        public int GetEffectiveKeyBits()
        {
            return this.effectiveKeyBits;
        }

        public byte[] GetIV()
        {
            return this.iv;
        }
    }

    public class IvParameterSpec : AlgorithmParameterSpec
    {
        private byte[] iv;

        public IvParameterSpec(byte[] iv)
        {
            this.iv = iv;
        }
        public byte[] GetIV()
        {
            return this.iv;
        }
    }

    public class AlgorithmParameterSpec
    {
    }
}

