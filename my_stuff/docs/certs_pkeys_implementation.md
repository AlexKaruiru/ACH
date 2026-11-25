# Clearing Security Implementation Guide (Simplified Configuration)

This document explains the encryption and digital signing implementation in the Clearing system. The configuration has been simplified to a 3-flag system to support all required client security scenarios.

## 1. Primary Configuration Flags

The behavior of the security layer is controlled by these three keys in the `App.config`:

| Key                | Type | Description                                                               |
| :----------------- | :--- | :------------------------------------------------------------------------ |
| `Sign`             | bool | Whether to digitally sign the file (valid for both GPG and Certificates). |
| `EnableEncryption` | bool | Whether to encrypt the file (valid for both GPG and Certificates).        |
| `UseCertificate`   | bool | Toggle between **Certificates/CMS** (true) and **OpenPGP/GPG** (false).   |

## 2. Security Profile Matrix

Clients and banks can choose one of four primary security profiles by setting the flags as shown below:

| Profile                  | `Sign`  | `EnableEncryption` | `UseCertificate` | Resulting Security Action                         |
| :----------------------- | :------ | :----------------- | :--------------- | :------------------------------------------------ |
| **No Security**          | `false` | `false`            | N/A              | File remains as clear text.                       |
| **Authentication Only**  | `true`  | `false`            | `false`          | Standalone PGP Digital Signature.                 |
| **Authentication Only**  | `true`  | `false`            | `true`           | CMS SignedData (Digital Signature).               |
| **Confidentiality Only** | `false` | `true`             | `false`          | PGP Encryption (No signature).                    |
| **Confidentiality Only** | `false` | `true`             | `true`           | CMS EnvelopedData (Encryption).                   |
| **Maximum Security**     | `true`  | `true`             | `false`          | **Sign-then-Encrypt** (OpenPGP/GPG Standard).     |
| **Maximum Security**     | `true`  | `true`             | `true`           | **Sign-then-Encrypt** (Certificate/CMS Standard). |

## 3. Configuration Details

| Property              | Description                                                                       |
| :-------------------- | :-------------------------------------------------------------------------------- |
| `PrivateKeyPath`      | Path to the sender's Private Key (for signing) or Certificate (.p12).             |
| `PublicKeyPath`       | Path to the recipient's Public Key (for encryption) or Public Certificate (.cer). |
| `CertificatePassword` | Password for the certificate store (if using P12 files).                          |

## 4. GPG Interoperability

When `UseCertificate` is `false`, the system uses the OpenPGP (GPG) format. Files generated can be handled by standard tools:

- **To Verify**: `gpg --verify <filename>`
- **To Decrypt**: `gpg --decrypt <filename>`

## 5. Test Case Scenarios

### 5.1 Test Commands

The `ACHCryptoTool.exe` supports the following operations:

| Command          | Syntax                                                           | Description                        |
| :--------------- | :--------------------------------------------------------------- | :--------------------------------- |
| **Encrypt Text** | `.\ACHCryptoTool.exe encrypt "plaintext"`                        | Encrypts a plain text string       |
| **Decrypt Text** | `.\ACHCryptoTool.exe decrypt "BASE64_STRING"`                    | Decrypts a Base64 encrypted string |
| **Encrypt File** | `.\ACHCryptoTool.exe encryptfile "source.txt" "destination.enc"` | Encrypts a file                    |
| **Decrypt File** | `.\ACHCryptoTool.exe decryptfile "source.enc" "destination.txt"` | Decrypts a file                    |

### 5.2 Test Scenario 1: Text String Encryption/Decryption

#### Prerequisites:

- Valid certificate/PFX file configured
- Correct certificate password

#### Test Steps:
