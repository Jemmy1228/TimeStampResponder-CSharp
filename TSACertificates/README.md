### TSACertificates

If you want to use your own Time-Stamp-Authority-Certificate, please pay attention!
A TSA Certificate must contain ExtendedKeyUsage X509Extension.
ExtendedKeyUsage only allows the cert for TimeStamp, and ExtendedKeyUsage must be marked with critical.

What's more, if your TSACert is not SelfSigned (I mean IssuerDN=SubjectDN) , you should paste the Intermediate Certificate after your TSACertificate to provide a full cert chain. Just like what we do when deploying a SSL certificate.
And remember that the private key must be in PKCS1 form (PKCS8 won't work)

Certs in this folder can be used directly for the program.
Just copy and put them in the folder of Demo.exe

These certificates are just an example, which is issued from my own PKI. You can trust my root certificate by importing EVRootCA.crt or run EVRootCA.reg. Certainly, you don't have to trust it...
The private key of TSA Certificates are uploaded as well. Though the certificate is not trusted by default, I hope you won't abuse them.

You can choose either cert in SHA1 folder or SHA256 folder. These two certificates are signed with different signature algorithms. Microsoft says that Windows XPs don't recognize certs hashed and signed with SHA256, so that SHA1 certs is more compatiable than SHA256. On newer systems, both of these certs would work.