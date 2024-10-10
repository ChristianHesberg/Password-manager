## Starting the app

- cd "password manager"
- dotnet ef database update
- start project

## Security flow
![./img.png](img.png)

## Security considerations
- A user has a master password
- The master password is hashed and salted and stored in a db using pbkdf2 key derivation
- The user must enter the master password in order to enter the program
- When the user logs in, the entered password and a different salt are used to derive a vault key
- The vault key is used to encrypt and decrypt data stored in the vault 