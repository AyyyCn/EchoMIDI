# 🎹 EchoMIDI – Rhythm Memory Game

An **experimental musical‑memory game** built with **Unity (URP)** and **MIDI**.
Listen to an ever‑growing piano sequence, then replay it on your **physical MIDI keyboard** or the **on‑screen keys**.

---

## 🚀 Feature Highlights

| Category                       | Details                                                                                         |
| ------------------------------ | ----------------------------------------------------------------------------------------------- |
| 🎧 **Guided Mode**             | Classic memory‑chain gameplay ("Simon" with real notes).                                        |
| ⚙️ **Settings**          | In‑game menu lets you tweak tempo, sequence length, velocity gate, etc. on the fly.             |
| 🎹 **Universal MIDI Support**  | Works with any class‑compliant device via **Midi Jack**, or the built‑in virtual keyboard.      |
| 🌟 **Interactive Keyboard UI** | Velocity‑sensitive glow, smooth tweens, particle sparkles on each hit.                          |
| 🏆 **Scoring**                 | Live **Score**, **Current Streak**, **Best Streak** HUD.                                        |

---

##  Screenshots

![image](https://github.com/user-attachments/assets/04a5e1f4-577f-446b-9f84-5c6bd3e95860)
![image](https://github.com/user-attachments/assets/3592ea2e-7ac8-47a0-9abd-4f86cfb9131e)


* **Guided Mode** – launches the core game (current build).
* **Settings** – open the live panel to fine‑tune gameplay variables before playing.

---



## ⚙️ Gameplay Parameters

All tunable variables are exposed in **Settings → Apply** and stored in a global **`GameConfig`** object read by `MelodyManager`.

| Setting                   | Field (Script)                | Default     |
| ------------------------- | ----------------------------- | ----------- |
| Delay Between Notes       | `delayBetweenNotes`           | `0.6` s     |
| Input Timeout             | `inputTimeout`                | `4` s       |
| Min Notes                 | `minNotes`                    | `3`         |
| Max Notes                 | `maxNotes`                    | `10`        |
| Step                      | `step` (+ per success)        | `1`         |
| Enable Velocity Threshold | `enableVelocityThreshold`     | `false`     |
| Min / Max Velocity        | `minVelocity` / `maxVelocity` | `0.1 / 1.0` |
| Skip Reset On Wrong       | `skipResetOnWrongInput`       | `false`     |

> Settings persist only for the current play session (static runtime asset).

---

## 🔧 Technical Overview

| Aspect            | Implementation                                                  |
| ----------------- | --------------------------------------------------------------- |
| **Unity Version** | 2022.3 LTS (URP template)                                       |
| **MIDI Input**    | [Keijiro – **Midi Jack**](https://github.com/keijiro/MidiJack)  |
| **Blur Effect**   | URP **Fullscreen Render Feature** (Kawase) + `RawImage` overlay |
| **UI**            | Unity UGUI – Keyboard, Timers, Panels (TMP for text)            |
| **Audio**         | Individual WAV clips per note in `Resources/wav/`               |
| **Data**          | Melody JSON in `Resources/melody.json`                          |

---





## 🛠️ Customisation 

1. **Change the Melody**
   • Edit `Resources/melody.json` (MIDI numbers & clip names).
   • Place matching WAVs in `Resources/wav/`.

2. **Reskin the UI**
   • Swap fonts, colors, sprites on `KeyboardKey` prefab.
   • Replace particle sprites to change hit‑effects.

3. **Tweak Game Feel**
   • Use Settings panel or edit defaults in `GameConfig` for timings & difficulty.

4. **Add New Modes**
   • Duplicate `GameScene`, adjust `MelodyManager` for endless/freeplay, or compose variants by subclassing.

---

## 🔗 Dependencies

* [Midi Jack](https://github.com/keijiro/MidiJack) (MIT)
* Unity TextMeshPro (Package Manager)

---

## 📜 License & Usage

Released for educational & experimental purposes.
Fork it, remix it, ship it

---

