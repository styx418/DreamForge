-- DreamCraftServer - Schéma complet de base de données
-- Version : 0.0.2 - Date : 2025-06-09

-- Table: accounts
CREATE TABLE accounts (
    id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(32) NOT NULL UNIQUE,
    email VARCHAR(128),
    password VARCHAR(128) NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    is_banned TINYINT(1) DEFAULT 0,
    is_active TINYINT(1) DEFAULT 1,
    account_level TINYINT DEFAULT 0,
    last_loggin DATETIME,
    online TINYINT(1) DEFAULT 0
);

-- Table: characters
CREATE TABLE characters (
    Character_id INT AUTO_INCREMENT PRIMARY KEY,
    acc_id INT NOT NULL,
    Player_Name VARCHAR(32) NOT NULL UNIQUE,
    Player_Skin VARCHAR(32) DEFAULT 'HumanMale',
    Level INT DEFAULT 1,
    Xp INT DEFAULT 0,
    XpToLevel INT DEFAULT 100,
    Pos_X FLOAT DEFAULT 0,
    Pos_Y FLOAT DEFAULT 0,
    Pos_Z FLOAT DEFAULT 0,
    Str INT DEFAULT 10,
    Endu INT DEFAULT 10,
    AC INT DEFAULT 0,
    Agil INT DEFAULT 10,
    Intel INT DEFAULT 10,
    Sages INT DEFAULT 10,
    FOREIGN KEY (acc_id) REFERENCES accounts(id) ON DELETE CASCADE
);

-- Table: item_types
CREATE TABLE item_types (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(32) NOT NULL,
    category VARCHAR(32),
    description TEXT
);

-- Table: items
CREATE TABLE items (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(64),
    description TEXT,
    type_id INT,
    FOREIGN KEY (type_id) REFERENCES item_types(id)
);

-- Table: item_stats
CREATE TABLE item_stats (
    id INT AUTO_INCREMENT PRIMARY KEY,
    item_id INT,
    stat_name VARCHAR(32),
    stat_value INT,
    FOREIGN KEY (item_id) REFERENCES items(id)
);

-- Table: inventory
CREATE TABLE inventory (
    id INT AUTO_INCREMENT PRIMARY KEY,
    character_id INT,
    item_id INT,
    quantity INT DEFAULT 1,
    FOREIGN KEY (character_id) REFERENCES characters(Character_id),
    FOREIGN KEY (item_id) REFERENCES items(id)
);

-- Table: equipment
CREATE TABLE equipment (
    id INT AUTO_INCREMENT PRIMARY KEY,
    character_id INT,
    slot VARCHAR(32),
    item_id INT,
    FOREIGN KEY (character_id) REFERENCES characters(Character_id),
    FOREIGN KEY (item_id) REFERENCES items(id)
);

-- Table: character_quests
CREATE TABLE character_quests (
    character_id INT,
    quest_id INT,
    status VARCHAR(32),
    progress INT,
    PRIMARY KEY (character_id, quest_id)
);

-- Table: guilds
CREATE TABLE guilds (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(64) NOT NULL,
    description TEXT
);

-- Table: guild_members
CREATE TABLE guild_members (
    guild_id INT,
    character_id INT,
    rank VARCHAR(32),
    PRIMARY KEY (guild_id, character_id),
    FOREIGN KEY (guild_id) REFERENCES guilds(id),
    FOREIGN KEY (character_id) REFERENCES characters(Character_id)
);

-- Table: character_flags
CREATE TABLE character_flags (
    character_id INT,
    flag_name VARCHAR(64),
    flag_value TINYINT(1) DEFAULT 0,
    PRIMARY KEY (character_id, flag_name),
    FOREIGN KEY (character_id) REFERENCES characters(Character_id)
);

-- Table: market_listings
CREATE TABLE market_listings (
    id INT AUTO_INCREMENT PRIMARY KEY,
    seller_id INT,
    item_id INT,
    quantity INT,
    price INT,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Table: inbox_messages
CREATE TABLE inbox_messages (
    id INT AUTO_INCREMENT PRIMARY KEY,
    character_id INT,
    subject VARCHAR(128),
    body TEXT,
    sent_at DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Table: chests
CREATE TABLE chests (
    id INT AUTO_INCREMENT PRIMARY KEY,
    location VARCHAR(64)
);

-- Table: chest_items
CREATE TABLE chest_items (
    chest_id INT,
    item_id INT,
    quantity INT,
    FOREIGN KEY (chest_id) REFERENCES chests(id),
    FOREIGN KEY (item_id) REFERENCES items(id)
);

-- Table: trades
CREATE TABLE trades (
    id INT AUTO_INCREMENT PRIMARY KEY,
    player1_id INT,
    player2_id INT,
    status VARCHAR(32)
);

-- Table: trade_items
CREATE TABLE trade_items (
    trade_id INT,
    item_id INT,
    from_player_id INT,
    quantity INT
);

-- Table: chat_logs
CREATE TABLE chat_logs (
    id INT AUTO_INCREMENT PRIMARY KEY,
    sender_id INT,
    message TEXT,
    sent_at DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Table: npcs
CREATE TABLE npcs (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(64),
    dialogue TEXT
);

-- Table: monsters
CREATE TABLE monsters (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(64),
    level INT,
    hp INT
);

-- Table: vendors
CREATE TABLE vendors (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(64)
);

-- Table: vendor_items
CREATE TABLE vendor_items (
    vendor_id INT,
    item_id INT,
    price INT
);

-- Table: spells
CREATE TABLE spells (
    SpellID INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(64) NOT NULL,
    Description TEXT,
    IconPath VARCHAR(128),
    ManaCost INT DEFAULT 0,
    Cooldown FLOAT DEFAULT 1.5,
    CastTime FLOAT DEFAULT 0,
    `Range` FLOAT DEFAULT 0,
    Power INT DEFAULT 0,
    SpellType ENUM('Damage','Heal','Buff','Debuff','Teleport') DEFAULT 'Damage',
    TargetType ENUM('Self','Enemy','Ally','Area') DEFAULT 'Enemy',
    ScriptName VARCHAR(64)
);

-- Table: character_spells
CREATE TABLE character_spells (
    CharacterID INT NOT NULL,
    SpellID INT NOT NULL,
    Level INT DEFAULT 1,
    PRIMARY KEY (CharacterID, SpellID),
    FOREIGN KEY (CharacterID) REFERENCES characters(Character_id) ON DELETE CASCADE,
    FOREIGN KEY (SpellID) REFERENCES spells(SpellID) ON DELETE CASCADE
);

-- Table: zones
CREATE TABLE zones (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(64),
    description TEXT
);

-- Table: maps
CREATE TABLE maps (
    id INT AUTO_INCREMENT PRIMARY KEY,
    zone_id INT,
    name VARCHAR(64),
    FOREIGN KEY (zone_id) REFERENCES zones(id)
);

-- Table: quests
CREATE TABLE quests (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(64),
    description TEXT
);

-- Table: loot_tables
CREATE TABLE loot_tables (
    id INT AUTO_INCREMENT PRIMARY KEY,
    source_type VARCHAR(32),
    source_id INT
);

-- Table: events
CREATE TABLE events (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(64),
    description TEXT,
    start_time DATETIME,
    end_time DATETIME
);

-- Table: logs
CREATE TABLE logs (
    id INT AUTO_INCREMENT PRIMARY KEY,
    type VARCHAR(32),
    message TEXT,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP
);