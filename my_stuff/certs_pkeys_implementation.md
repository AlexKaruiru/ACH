# Certificates and Public/Private Keys Implementation

## Changes Made

### 1. Dynamic Key Loading
- **Public and Private Keys**:
  - Removed hardcoded keys from the `CryptographyHelper` class.
  - Keys are now dynamically loaded from the configuration file using the following keys:
    - `PublicKeyPath`
    - `PrivateKeyPath`

### 2. Certificate Support
- **Optional Certificate Usage**:
  - Introduced support for certificates for encryption and decryption.
  - Certificates are dynamically loaded using the following configuration keys:
    - `CertificatePath`
    - `CertificatePassword`
    - `UseCertificate` (to toggle between keys and certificates).

### 3. Support for Multiple Algorithms
- **Asymmetric Algorithms**:
  - Added support for RSA (default).
  - Configurable via the `AsymmetricAlgorithm` key in the configuration file:
    ```xml
    <add key="AsymmetricAlgorithm" value="RSA"/> <!-- Options: RSA -->
    ```
- **Hashing Algorithms**:
  - Added support for SHA-256 (default).
  - Configurable via the `HashingAlgorithm` key in the configuration file:
    ```xml
    <add key="HashingAlgorithm" value="SHA-256"/> <!-- Options: SHA-256 -->
    ```

### 4. Logging
- Added logging to:
  - Indicate whether signing is enabled or disabled.
  - Specify the signing method (keys or certificates).
  - Log the selected asymmetric and hashing algorithms.
  - Log whether the required keys or certificates were found.

### 5. Backward Compatibility
- Retained hardcoded keys as a fallback if configuration keys are not provided.

### 6. Symmetric Encryption Support
- **Shared Key**:
  - Added support for symmetric encryption and decryption using a shared key.
  - The shared key is dynamically loaded from the configuration file using the `SharedKey` key:
    ```xml
    <add key="SharedKey" value="YourSharedSymmetricKeyHere"/>
    ```
  - Symmetric encryption is implemented using AES.

- **Methods Added**:
  - `EncryptWithSharedKey`: Encrypts data using the shared key.
  - `DecryptWithSharedKey`: Decrypts data using the shared key.

- **Configuration Example**:
  ```xml
  <appSettings>
      <add key="SharedKey" value="YourSharedSymmetricKeyHere"/>
  </appSettings>
  ```

- **Logging**:
  - Logs whether the shared key is configured and used for encryption/decryption.

---

## Current Certificate Implementation

### Features
1. **Encryption and Decryption**:
   - Certificates are used for encrypting and decrypting files.
   - RSA is the default algorithm for certificate-based operations.

2. **Configuration**:
   - Certificates are configured using the following keys:
     ```xml
     <add key="CertificatePath" value="path_to_certificate.pfx"/>
     <add key="CertificatePassword" value="password"/>
     <add key="UseCertificate" value="true"/>
     ```

3. **Logging**:
   - Logs indicate whether certificates are being used and whether they are found.

4. **Hashing**:
   - SHA-256 is used for hashing to ensure file integrity.

---

## Future Enhancements

### 1. Support for Additional Algorithms
- **Asymmetric Algorithms**:
  - Add support for ECC (Elliptic Curve Cryptography) for environments requiring smaller keys and faster performance.
  - Example: NIST P-256 (prime256v1) or Ed25519.
- **Hashing Algorithms**:
  - Add support for SHA-3 for high-security environments.

### 2. Key Rotation
- Implement automatic key rotation to enhance security.
- Notify users when keys or certificates are about to expire.

### 3. Enhanced Certificate Formats
- Add support for `.pem` and `.crt` formats in addition to `.pfx`.
- Use libraries like `BouncyCastle` for parsing and handling these formats.

### 4. Secure Key Exchange
- Implement a secure mechanism for exchanging keys and certificates during setup.
- Periodically update keys to prevent breaches.

### 5. Integration with MFI Module
- Ensure seamless integration for:
  - **Incoming Leg**:
    - Decrypt and process files.
    - Verify file integrity using signed hashes.
  - **Outgoing Leg**:
    - Encrypt files using the receiver's public key.
    - Sign the file hash for integrity verification.

### 6. Shared Key Configuration
- Added the following key to all relevant configuration files:
  ```xml
  <add key="SharedKey" value="YourSharedSymmetricKeyHere"/>
  ```
- Updated files:
  - `TzIncomingClearingService\App.config`
  - `BRNETUploadDownload\app.config`
  - `BRRTGSProcessing\App.config`
  - `Common\app.config`
  - `BRNETUploadDownload\bin\Debug\BRNETUploadDownload.exe.config`

---

## Scope of Work Analysis

### In-Scope Items
- Outward Cheques/EFTs
- Inward Cheques/EFTs
- Reporting

### Encryption Process
- **Key Generation**:
  - Generate public/private key pairs using RSA (2048-bit or higher).
- **Message Encryption**:
  - Use AES256 for encrypting messages.
- **Hashing**:
  - Implement SHA256 for message integrity.
- **File Processing**:
  - Encrypt outgoing files with the receiver's public key.
  - Decrypt incoming files with the bank's private key.

### Code Changes
- Modify the ACH File Module:
  - Add encryption and decryption logic.
  - Implement wrapper classes for key and certificate operations.
- Integration with MFI Module:
  - Process incoming and outgoing files securely.
  - Generate detailed reports for all transactions.

---

This document outlines the current certificate implementation and the roadmap for future improvements to ensure secure file encryption and decryption in compliance with the outlined requirements.