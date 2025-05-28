# Drone Base Control System (Unity Project)

This Unity project implements a controllable base system with dynamically spawned drones. Each base belongs to a faction (Red or Blue) and can spawn, remove, or adjust drones based on the desired count and speed. It includes logic for choosing and destroying the farthest drone when drone count exceeds the target count and a simple colliding system for drones.

---

## 🎮 Features

- ✅ Dynamic drone spawning based on configurable values
- ✅ Real-time drone speed adjustment (with help of UI slider element)
- ✅ Automatic removal of farthest drones when reducing drone count
- ✅ Avoidance logic for drones to not overlap
- ✅ UI integration to display base strength
- ✅ Manual and programmatic adjustments via the `SetDroneSettings()` method

---

## 📂 Project Structure

- `BaseController.cs` — Controls the base, drone count, and their logic
- `DroneController.cs` — Handles drone behavior, movement, and speed
- `GameplayUIHandler.cs` — UI update manager
- `AsteroidScript.cs` — Optional component for drones to avoid overlap
- `AsteroidManager.cs` — Interface that gets, regiters new astroids and unregisters depleted astroids
- `AsteroidSpawnerManager.cs` — Settings and configuration for spawning asteroids

---

## 🛠️ Setup Instructions

1. Clone the repository.
2. Open the project in Unity 2022.3 or newer. (Created in Unity Editor Version 6000.0.33f1)
3. Make sure all scenes, prefabs, and scripts are in the correct folders (`Assets/Scripts`, `Assets/Prefabs`, `Assets/UI`, etc.).
4. Press Play. You can adjust drone settings via the UI or sliders in the editor.

---

## 📄 License

Free to use, modify, and distribute.
