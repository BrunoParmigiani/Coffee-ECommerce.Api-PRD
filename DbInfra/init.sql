DROP TABLE IF EXISTS "Administrators";
DROP TABLE IF EXISTS "Announcements";
DROP TABLE IF EXISTS "ApiAccess";
DROP TABLE IF EXISTS "Establishments";
DROP TABLE IF EXISTS "Favorites";
DROP TABLE IF EXISTS "FooterItems";
DROP TABLE IF EXISTS "NewOrders";
DROP TABLE IF EXISTS "Products";
DROP TABLE IF EXISTS "Reports";
DROP TABLE IF EXISTS "Orders";
DROP TABLE IF EXISTS "Users";

CREATE TABLE "Administrators" (
	"Id" UUID PRIMARY KEY,
	"Name" VARCHAR(100) NOT NULL,
	"Email" VARCHAR UNIQUE NOT NULL,
	"CPF" VARCHAR(11) UNIQUE NOT NULL,
	"Password" VARCHAR NOT NULL,
	"PhoneNumber" VARCHAR(13) NOT NULL,
	"RefreshToken" VARCHAR,
	"RefreshTokenExpireTime" TIMESTAMP NOT NULL,
	"Role" VARCHAR(20),
	"Blocked" BOOLEAN NOT NULL,
	"DeleteSchedule" TIMESTAMP NOT NULL,
	"EstablishmentId" UUID
);

CREATE TABLE "Announcements" (
	"Id" UUID PRIMARY KEY,
	"CreatorId" UUID NOT NULL,
	"Name" VARCHAR(100) NOT NULL,
	"Message" VARCHAR NOT NULL,
	"Recipients" VARCHAR NOT NULL
);

CREATE TABLE "ApiAccess" (
	"Id" UUID PRIMARY KEY,
	"ServiceName" VARCHAR(100) NOT NULL,
	"Key" VARCHAR
);

CREATE TABLE "Establishments" (
	"Id" UUID PRIMARY KEY,
	"Name" VARCHAR(100) NOT NULL,
	"Email" VARCHAR UNIQUE NOT NULL,
	"Address" VARCHAR NOT NULL,
	"Complement" VARCHAR(100) NOT NULL,
	"CNPJ" VARCHAR(14) NOT NULL,
	"AdministratorId" UUID NOT NULL,
	"PhoneNumber" VARCHAR(13) NOT NULL,
	"Password" VARCHAR NOT NULL,
	"RefreshToken" VARCHAR,
	"RefreshTokenExpireTime" TIMESTAMP NOT NULL,
	"Role" VARCHAR(20),
	"Blocked" BOOLEAN NOT NULL,
	"DeleteSchedule" TIMESTAMP NOT NULL
);

CREATE TABLE "Favorites" (
	"Id" UUID PRIMARY KEY,
	"UserId" UUID NOT NULL,
	"ProductId" UUID NOT NULL
);

CREATE TABLE "FooterItems" (
	"Id" UUID PRIMARY KEY,
	"Name" VARCHAR(100) NOT NULL,
	"Link" VARCHAR(255) NOT NULL
);

CREATE TABLE "NewOrders"(
	"Id" UUID PRIMARY KEY,
	"UserId" UUID NOT NULL,
	"PaymentMethod" INT,
	"EstablishmentId" UUID NOT NULL,
	"DeliveryTime" INTERVAL NOT NULL,
	"Items" VARCHAR NOT NULL,
	"DeniedOrder" BOOLEAN,
	"UserName" VARCHAR NOT NULL,
	"UserAddress" VARCHAR NOT NULL,
	"UserComplement" VARCHAR NOT NULL,
	"DeniedReason" VARCHAR(500)
);

CREATE TABLE "Products" (
	"Id" UUID PRIMARY KEY,
	"CreatorId" UUID NOT NULL,
	"Name" VARCHAR(100) NOT NULL,
	"Price"  REAL NOT NULL,
	"Description" VARCHAR(255) NOT NULL,
	"EstablishmentId" UUID NOT NULL
);

CREATE TABLE "Reports" (
	"Id" UUID PRIMARY KEY,
	"UserId" UUID NOT NULL,
	"EstablishmentId" UUID NOT NULL,
	"OrderId" UUID NOT NULL,
	"OpenTime" TIMESTAMP NOT NULL,
	"CloseTime" TIMESTAMP NOT NULL,
	"Problem" INT NOT NULL,
	"Description" VARCHAR NOT NULL,
	"Status" INT NOT NULL
);

CREATE TABLE "Orders" (
	"Id" UUID PRIMARY KEY,
	"UserId" UUID NOT NULL,
	"TotalValue" REAL NOT NULL,
	"PaymentMethod" INT NOT NULL,
	"Paid" BOOLEAN NOT NULL,
	"Delivered" BOOLEAN NOT NULL,
	"EstablishmentId" UUID NOT NULL,
	"DeliveryTime" INTERVAL NOT NULL,
	"DeliveredAtTime" TIMESTAMP NOT NULL,
	"OrderedAt" TIMESTAMP NOT NULL,
	"Items" VARCHAR NOT NULL,
	"Rated" BOOLEAN NOT NULL,
	"TimeRating" INT,
	"QualityRating" INT,
	"UserComments" VARCHAR(500),
	"DeniedOrder" BOOLEAN NOT NULL,
	"DeniedReason" VARCHAR(500)
);

CREATE TABLE "Users" (
	"Id" UUID PRIMARY KEY,
	"Name" VARCHAR(100) NOT NULL,
	"Email" VARCHAR UNIQUE NOT NULL,
	"CPF" VARCHAR(11) UNIQUE NOT NULL,
	"Password" VARCHAR NOT NULL,
	"PhoneNumber" VARCHAR(13) NOT NULL,
	"Address" VARCHAR NOT NULL,
	"Complement" VARCHAR(100) NOT NULL,
	"RefreshToken" VARCHAR,
	"RefreshTokenExpireTime" TIMESTAMP NOT NULL,
	"Role" VARCHAR(20),
	"Suspended" BOOLEAN NOT NULL,
	"DeleteSchedule" TIMESTAMP NOT NULL
);


INSERT INTO "Administrators" (
	"Id",
	"Name",
	"Email",
	"CPF",
	"Password",
	"PhoneNumber",
	"RefreshToken",
	"RefreshTokenExpireTime",
	"Role",
	"Blocked",
	"DeleteSchedule",
	"EstablishmentId")
VALUES (
	'5e9c03d2-8fbd-416c-bc77-d66e96b27af7',
	'Bruno Henrique Parmigiani Caetano',
	'owner@expcafe.com',
	'47780031870',
	'4c1029697ee358715d3a14a2add817c4b01651440de808371f78165ac90dc581',
	'00910101010',
	'XrIHk5KUoJrvtPdZ05XXxSBhpqhUKD/rm7Q+NZXn9jY=',
	'2024-11-19 23:12:02.068017',
	'business_owner',
	'false',
	'infinity',
	'00000000-0000-0000-0000-000000000000'
);

INSERT INTO "ApiAccess" ("Id", "ServiceName", "Key") VALUES
('62ec731d-a220-4cf7-9d89-fea660fd14bc', 'frontend', '12aa37b319fcd21ab10f9149fcd3177dd0b96058416a7623926c3211abde3119'),
('7cdce343-54b0-41b2-8ce8-7f02deeefb2b', 'com_module', 'f40afc684813677f71dbf6951c36bc47d6b62cf75dfb02ea8ba384a2d4b3f89d');