# Elden Ring Twitch Integration
## Overview
This project enables Twitch viewers to interact with the game Elden Ring using custom channel point rewards. With this integration, viewers can modify the game's in-memory stats—such as vigor, endurance, spirit, strength, dexterity, intelligence, faith, and esotericism—or affect the player's health by redeeming rewards in Twitch chat.

**Warning: This project uses memory manipulation techniques to alter game data. Use it responsibly and only with your Elden Ring anti cheat disabled**

## Features
### Custom Reward Integration:
Listens for Twitch custom reward redemptions via TwitchLib and triggers in-game actions.

### In-Game Stat Modification:
Processes commands like vigueur+1 or foi-1 from the chat to increase or decrease stats.

### Health Management:
Viewers can use rewards to deal damage or heal the character, dynamically modifying the in-game health.

### File Logging:
Updates text files in the user's Documents folder with current stat values for tracking purposes.

### User-Friendly Interface:
A simple Windows Forms interface allows you to start the bot and monitor its activity.

## How It Works
### Twitch Connection:
The bot connects to a specified Twitch channel using TwitchLib. It monitors chat messages and custom reward redemptions triggered by channel points.

## Command Parsing:
When a reward is redeemed, the bot parses the command (e.g., vigueur+1 or foi-1) to determine the stat to modify and whether to increase or decrease its value.

Memory Manipulation:
The project uses Windows API functions (ReadProcessMemory and WriteProcessMemory) to read and write values directly into the Elden Ring process memory. This allows real-time updates to the in-game stats.

Feedback Mechanism:
The bot sends a confirmation message back to the Twitch chat, informing viewers of the change made.
