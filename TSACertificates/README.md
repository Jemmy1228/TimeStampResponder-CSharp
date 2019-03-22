### TSACertificates

Time-Stamp-Authority-Certificates in this folder can be used directly for the program.
Just copy and put them in the folder of Demo.exe

If you want to use your own cert, please pay attention!
A TSA Certificate must contain ExtendedKeyUsage X509Extension.
ExtendedKeyUsage only allows the cert for TimeStamp, and ExtendedKeyUsage must be marked with critical.

These certificates are just an example, which is issued from my own PKI. You can trust my root certificate by importing EVRootCA.crt or run EVRootCA.reg. Certainly, you don't have to trust it...
The private key of TSA Certificates are uploaded as well. Though the certificate is not trusted by default, I hope you won't abuse them.

You can choose either cert in SHA1 folder or SHA256 folder. These two certificates are signed with different signature algorithms. Microsoft says that Windows XPs don't recognize certs hashed and signed with SHA256, so that SHA1 certs is more compatiable than SHA256. On newer systems, both of these certs would work.