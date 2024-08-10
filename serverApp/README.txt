так выгледят мои секреты:
{
  "UserSecrets": {
    "Email": {
      "Address": "example@yandex.ru",/*Яндекс потому что пидарасы из гугл посчитали что я из Рф по моему номеру, смешные номера Казахстана короче*/
      "Password": "examplePas"
    },
    "RedisConnectionStr": "127.0.0.1:6666",
    "PostgresConnectionStr": "Database=notesdb;Server=localhost;Port=5432;User Id = postgres;Password=examplePostgresPas",
    "Jwt": {
      "Issuer": "localhost",
      "AlgorithmForAccessToken": "HS256",
      "AccessTokenExpiresMinutes": 15,
      "AccessTokenSecretKey": "lksdjlkdsjfskdjflsdjflskjfklsdjfjwlekjwlkjlwkefnwlefnwelkfnwlkefnkwelfnwelkfnwelkfnwlnfwelfkwen",

      "AlgorithmForRefreshToken": "RS256",
      "RefreshTokenExpiresDays": 10,
      "RefreshTokenSecretKey": "lksdjlkdsjfskdjflsdjflskjfklsdjfjwlekjwlkjlwkefnwlefnwelkfnwlkefnkwelfnweysecretKeysecretKey"
    }
  }
}

