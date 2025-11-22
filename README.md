# ⚔️ 3D RPG. A third-person open-world RPG prototype developed as my **bachelor’s project**.  
The game demonstrates a full set of core RPG mechanics: combat, magic, AI enemies, open world exploration, dialog trees, inventory, talisman system, progression, VFX, SFX, and an optimized architecture built in Unity.

This prototype serves as a foundation for a future full-scale RPG and showcases my practical skills in gameplay programming, level design, UI/UX, optimization, and systems engineering.

---

## 🎥 Gameplay Preview

<p align="center">
  <img src="TH1.png"/>
</p>
<p align="center">
  <a href="https://youtu.be/QdU7WAkmksI" target="_blank">
    <img src="https://img.shields.io/badge/Watch_on_YouTube-FF0000?style=for-the-badge&logo=youtube&logoColor=white"/>
  </a>
</p>


---

## 🎮 About the Project

The game includes all essential gameplay systems typical for modern RPG titles:

- open world exploration  
- melee combat system  
- elemental magic system  
- dynamic enemy AI  
- inventory & loot  
- dialog trees  
- progression and leveling  
- talismans with elemental bonuses  
- interaction system (chests, NPCs, objects)  

The project draws inspiration from **Skyrim**, **The Witcher 3**, **Dark Souls**, and **Genshin Impact**, while offering a custom lightweight architecture tailored for a small development team.

---

## 🧩 Core Mechanics & Systems

### 🗡️ Combat System
- Event-based logic without heavy Update() usage  
- Hit detection via OverlapBox/Sphere  
- Animation events triggering attacks  
- Combo timing, cooldowns via coroutines  
- Enemy behavior: aggro → chase → attack → retreat → death  

### 🔥 Magic & Elements
- Four elements: water, fire, earth, air  
- Each has unique VFX/SFX  
- Magic is integrated into combat and talismans  

### 🧿 Talismans
- Two parameters:  
  - **Element** (4 types)  
  - **Creature Level** (5 tiers)  
- Provide passive bonuses (damage, speed, defense, regen, etc.)  
- Loot rarity depends on chest type  

### 🎒 Inventory & Loot
- Chest system with rarity tiers  
- Random item generation upon opening  
- Unified interaction system via base Interactable class  
- UI for collected items  

### 💬 Dialog System
- Branching dialog trees  
- Conditions and variable states  
- Player choices impact world and quests  
- Non-linear storytelling framework  

### 🧠 Enemy AI
- Navigation via NavMesh  
- State-based logic: patrol, chase, attack, retreat  
- Optimized activation based on distance to player  

### 🏞️ World & Level Design
- Custom terrain (Unity Terrain)  
- Manual placement of buildings, nature, NPCs  
- Optimizations: LOD, occlusion culling, static batching  


---

## ⚙️ Technologies Used

- **Unity (C#)**  
- **Mixamo** — animations  
- **Photoshop** — texture & UI creation  
- **NavMesh** — AI navigation  
- **C# Events & Coroutines** — architecture foundation  

---

## 🚀 Performance & Optimization

To ensure stable FPS:

- Event-driven architecture  
- Coroutine-based cooldowns & logic  
- No unnecessary Update() calls  
- Enemy activation only when near player  
- Optimized VFX  
- Profiler-based performance tuning  

---
