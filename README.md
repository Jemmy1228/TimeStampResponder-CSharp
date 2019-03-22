### TimeStampResponder

This project is a TimeStampResponder Library with its Demo.

You can use my project to produce back-dated TimeStampResponse as a co-signature to validate expired/revoked code-sign signatures.

You must generate your own TSA certificate together with with its private key, and import the cert in to your trust store. There are some requirements for the cert-key pair, which is explainned in TSACertificates folder.

The Library supports both Microsoft Authenticode TimeStamp and RFC3161 TimeStamp.
The Demo is just a _**local**_ responder, so it can't be a reliable TimeStamp Server.