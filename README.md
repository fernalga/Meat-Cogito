# MEAT COGITO
*I Think, Therefore I Am... But What Am I?*  

> **A psychological horror game where UI glitches and body horror question your humanity.**  
> *"You answered 'Yes' to 'Are you human?'... But the system disagrees."*  

---

## 📜 **Project Dossier**  
| Document | Description |  
|----------|-------------|  
| [📄 Proposal](/Docs/Project_Proposal.pdf) | Original scope, timeline, and tools |  
| [🎮 Design Doc](/Docs/Game_Design_Document.pdf) | Mechanics, narrative, and USP |  
| [🎬 Storyboard](/Docs/Meat_Cogito_Story_Board.pdf) | Scene-by-scene breakdown |  
| [📊 Presentation](/Docs/Final_Presentation.pdf) | Development process & demo |  


## 🧠 **Abstract**  
**Meat Cogito** is a single-player, narrative-driven psychological horror game that merges **body horror** with **Cartesian philosophy**.  

### **The Premise**  
You awaken as a **disembodied brain** in a surreal simulation where:  
- **Flesh constructs reality** (walls pulse, NPCs whisper your deepest doubts)  
- **Every choice mutates your form** (build a "human" body... or something *else*)  
- **The system fights back** (glitching UI, meta crashes, *Project VERI* propaganda)  

### **The Questions**  
- *"If I think, am I?"*  
- *"Is my body real, or just a perception?"*  
- *"Will becoming ‘human’ save me... or trap me?"*  

### **The Experience**  
- **Grotesque 3D Environments:** Explore shifting organic spaces (Blender + Unity HDRP)  
- **Unsettling Interactions:** Rotate/toss objects with *Resident Evil*-inspired weight  
- **Three Endings:** Become a *True Being*, transcend as pure thought, or **shatter the simulation**  

### **Technical Execution**  
- **Solo-Developed** in Unity (C#) with custom:  
  - 3D models (Blender)  
  - VHS-style cutscenes (DaVinci Resolve)  
  - Glitch UI (Photoshop + Shader Graph)  
- **Philosophical Framework:** Choices reflect Descartes’ *cogito* paradoxes  

> *"A horror game that deletes itself—because the scariest ending is waking up."*  

---

## 🎥 **Features**  
### 1. **Unsettling UI/UX**  
- Glitching UI (shifting fonts, hex code messages)
![MainMenu (wip)](https://github.com/user-attachments/assets/23fbd7e2-6296-42d1-837f-bf47ebba6f2f)
![PauseMenu](https://github.com/user-attachments/assets/a39d1432-0332-47f0-84c2-ed98e6d4e0f1)
![{SettingsMenu}](https://github.com/user-attachments/assets/1ef8dbcc-364e-48bc-9ab4-8e4e18870c02)


- *Project VERI* VHS tape with creepy CAPTCHA tests


https://github.com/user-attachments/assets/336fa6e7-3ef8-49a7-bc36-cda200b4ca29

![{B47DD21B-D631-484D-9F1F-77C7C0EED55C}](https://github.com/user-attachments/assets/5a2399cc-7359-4ddc-b04e-1db174d25a96)


- Game "crashes" itself as narrative device  

### 2. **Player Avatar & Interaction**  
- 3D brain avatar with Blender-modeled animations  
- Objects react to being examined (pickup/rotate/toss)  
- Spaceship corridor meltdown sequence  

### 3. **Philosophical Terror**  
```csharp
// Example from Humanity Test:
new Question() {
    questionText = "7. Which of these is the real version of you?",
    answers = new string[] { "This one", "The reflection", "The one in the file", "None are left" }
}
```

## 🔧 Tech Stack

| Role            | Tool               | Badge |
|-----------------|--------------------|-------|
| Engine          | Unity (C#)         | [![Unity](https://img.shields.io/badge/Unity-2025-black?logo=unity)](https://unity.com) |
| IDE             | JetBrains Rider    | [![Rider](https://img.shields.io/badge/Rider-2024.3-white?logo=rider&logoColor=black&color=9cf)](https://jetbrains.com/rider/) |
| 3D Modeling     | Blender            | [![Blender](https://img.shields.io/badge/Blender-3.0+-orange?logo=blender)](https://blender.org) |
| UI/Textures     | Photoshop          | [![Photoshop](https://img.shields.io/badge/Photoshop-CC-blue?logo=adobe-photoshop)](https://adobe.com/photoshop) |
| Video           | DaVinci Resolve    | [![DaVinci Resolve](https://img.shields.io/badge/DaVinci_Resolve-20-black?logo=blackmagic-design)](https://blackmagicdesign.com/products/davinciresolve) |
| Audio           | FL Studio          | [![FL Studio](https://img.shields.io/badge/FL_Studio-21-purple?logo=fl-studio)](https://image-line.com) |

## 🌌 Storyboard Highlights

![{Storyboard Highlight}](https://github.com/user-attachments/assets/0c69f8b9-2286-43ee-ae29-a00313326977)


*"The walls breathe. Your choices make them bleed."*

## 🚧 Future Experiments

- **Dynamic Body Horror:** Link “humanity test” answers to grotesque transformations, your choices warp your brain into a monster, a human, or ????
- **Resident Evil-Inspired Puzzles:** rotate/toss objects to solve “glitches”
- **Meta:** Random “simulation breaks”—UI corrupts, Unity editor windows pop up in game, text file is saved on desktop with your test answers
- **Scene Expansion:** A new scene where walls pulse with veins, and NPCs whisper your test answers back at you


## ⁉️ FAQ

**Q:** Why does the game close abruptly?  
**A:** *"Reality is an unstable build."*

**Q:** How long is the demo?  
**A:** *~5 minutes of existential dread.*
