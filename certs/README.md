# certs

Generate Self-Signed Certificate with OpenSSL: 

```sh
# Generate private key
openssl genpkey -algorithm RSA -out server-key.pem -aes256

# Generate certificate signing request
openssl req -new -key server-key.pem -out server-csr.pem

# Generate self-signed certificate
openssl x509 -req -days 365 -in server-csr.pem -signkey server-key.pem -out server-cert.pem
```

**NOTE:** Ideally, perform this task within a separate Docker container running Ubuntu 20.04 and then transfer the artifacts to this location