# certs

If you need to refresh the certificates and private keys because they are expiring, follow these steps:

0. Generate **Self-Signed Certificate** with OpenSSL: 

```sh
# Generate private key
openssl genpkey -algorithm RSA -out server-key.pem

# Generate certificate signing request
openssl req -new -key server-key.pem -out server-csr.pem

# Generate self-signed certificate
openssl x509 -req -days 365 -in server-csr.pem -signkey server-key.pem -out server-cert.pem
```

1. Generate **Self-Signed CA Certificate** with OpenSSL: 

```sh
# Generate private key
openssl genpkey -algorithm RSA -out ca-key.pem

# Generate certificate signing request
openssl req -new -key ca-key.pem -out ca-csr.pem

# Generate self-signed certificate
openssl x509 -req -days 365 -in ca-csr.pem -signkey ca-key.pem -out ca-cert.pem
```

---
**NOTE:** Ideally, perform this task within a separate Docker container running Ubuntu 20.04 and then transfer the artifacts (private key and certificate) to this location