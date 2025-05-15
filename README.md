[![Downloads](https://img.shields.io/github/downloads/LaFesta1749/ParlamataCoinFlips/total?label=Downloads\&color=333333\&style=for-the-badge)](https://github.com/LaFesta1749/ParlamataCoinFlips/releases/latest)
[![Discord](https://img.shields.io/badge/Discord-Join-5865F2?style=for-the-badge&logo=discord&logoColor=white)](https://discord.gg/PTmUuxuDXQ)

# ParlamataCoinFlips

A custom plugin for **SCP: Secret Laboratory**, built with **Exiled 9.6.0-beta7**, that adds a unique risk-reward mechanic to the coin item. When players flip a coin, they receive a random good or bad outcome, ranging from healing and items to explosions and role swaps.

---

## 🔧 Features

* 🎲 **Heads or Tails system** – 50/50 chance on each flip.
* ✅ Good outcomes include:

  * Healing, bonus HP
  * Keycards, medical items, random guns
  * Speed boost, SCPs like 268 or 330
  * Teleportation to surface, role buffs, and more
* ❌ Bad outcomes include:

  * HP loss, teleport to Class-D spawn
  * Warhead toggles, live grenades, random SCP transformation
  * Instant death, role swap, inventory wipe, random teleport
* 📏 Size distortion effect (scale change)
* 💬 Hints shown using **HintServiceMeow v5.4.0 Beta 1**, centered at configurable Y-axis
* ⚙️ Fully configurable chance weights for each effect
* ⏳ Cooldown system and coin usage limits
* ⟳ Coin items have randomized use counts per round
* 🛡️ Configurable ignored roles, item pools, SCP targets, and more

---

## 📂 Installation

1. Place `ParlamataCoinFlips.dll` in your server's `Exiled/Plugins/` directory.
2. Make sure you are running **Exiled 9.6.0-beta7**.
3. Download and install **HintServiceMeow v5.4.0 Beta 2**.
4. Restart your server to generate the config file.

---

## ⚙️ Configuration (`config.yml`)

```yaml
good_events:
  keycard_chance: 20
  medical_kit_chance: 35
  teleport_to_escape_chance: 5
  heal_chance: 10
  bonus_hp_chance: 10
  ...

bad_events:
  hp_reduction_chance: 20
  tp_to_class_d_chance: 5
  bad_effect_chance: 20
  ...

global_settings:
  enable_cooldown: true
  cooldown_hint: "⏳ Please wait before flipping again."
  max_uses_per_round: 3
  max_uses_hint: "🚫 No more coin flips allowed this round."
  red_card_chance: 15
  items_to_give:
    - Adrenaline
    - GunE11SR
    - Coin
  valid_scps:
    - Scp049
    - Scp173
  rooms_to_teleport:
    - Surface
    - Lcz914
```

> **Note:** All chance values are weights, not true percentages. The system normalizes them internally.

---

## 🧠 Developer Notes

* Plugin uses `Player.Scale` for size change effects.
* Effects are executed via `EffectHandler.ExecuteCoinFlip(Player)`.
* Coin uses and cooldowns are tracked via `CoinUsesHandler`.
* Hints rendered via `HintManager` with configurable duration and Y-position.
* Uses Enum-based safe parsing for items and effects.

---

## 🧑‍💻 Author

**LaFesta1749**

---

## 📜 License

MIT – Do whatever you want, just don't remove credits.

---

## 🛠 Built With

* [Exiled](https://github.com/ExMod-Team/EXILED/releases) 9.6.0-beta7
* [HintServiceMeow](https://github.com/MeowServer/HintServiceMeow) v5.4.0 Beta 2
