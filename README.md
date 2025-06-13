[![Downloads](https://img.shields.io/github/downloads/LaFesta1749/ParlamataCoinFlips/total?label=Downloads&color=333333&style=for-the-badge)](https://github.com/LaFesta1749/ParlamataCoinFlips/releases/latest)
[![Discord](https://img.shields.io/badge/Discord-Join-5865F2?style=for-the-badge&logo=discord&logoColor=white)](https://discord.gg/PTmUuxuDXQ)

# ParlamataCoinFlips

**Version:** Custom Development Build

A chaotic and feature-rich plugin for SCP: Secret Laboratory where flipping a coin can trigger good or bad events, custom sounds, or absurd consequences. Designed for SCP Bulgaria ПАРЛАМАТА.

---

## ✅ Added Features

### 🔊 **Custom Coin Flip Sounds**

* **Heads** and **Tails** can have separate sounds.
* Uses `AudioPlayerApi` with 3D spatial audio.
* Configure sound filenames in:

  ```yml
  CoinFlipSounds:
    Enabled: true
    HeadSoundFile: "heads.mp3"
    TailSoundFile: "tails.mp3"
    HeadAutoDestroyDelay: 2.5
    TailAutoDestroyDelay: 2.5
  ```
* Place sound files in: `.../Configs/ParlamataCoinFlips/audio/`

---

## 🟢 Good Coin Effects

| Effect Name     | Description                                                                    |
| --------------- | ------------------------------------------------------------------------------ |
| `coin_theft`    | Steals a coin from another player (if any).                                    |
| `heal_allies`   | Heals nearby teammates. Radius + Heal amount configurable.                     |
| `door_effect`   | Unlocks or locks nearby doors randomly. Radius and durations are configurable. |
| `scp_summon`    | Random SCP (excluding 079) is summoned on top of the flipper.                  |
| `keycard`       | Gives the player a containment or facility manager keycard.                    |
| `medical_kit`   | Grants a medkit and painkillers.                                               |
| `tp_escape`     | Teleports player to escape area.                                               |
| `heal`          | Heals the player for 25 HP.                                                    |
| `bonus_hp`      | Adds 10% bonus HP.                                                             |
| `hat`           | Gives SCP-268.                                                                 |
| `good_effect`   | Random good status effect (e.g. speed, invisibility).                          |
| `logicer`       | Logicer with 1 ammo.                                                           |
| `lightbulb`     | Gives SCP-2176.                                                                |
| `pink_candy`    | Spawns SCP-330 with pink candy.                                                |
| `bad_revo`      | Gives a revolver with 1 bullet.                                                |
| `empty_hid`     | Gives a MicroHID with 0 charge.                                                |
| `force_respawn` | Triggers MTF arrival via CASSIE.                                               |
| `size_change`   | Changes player scale.                                                          |
| `random_item`   | Spawns random item from config.                                                |
| `speed_boost`   | Applies a movement boost effect.                                               |
| `anti_death`    | Grants pseudo-invincibility (placeholder only).                                |

---

## 🔴 Bad Coin Effects

| Effect Name         | Description                                                       |
| ------------------- | ----------------------------------------------------------------- |
| `coin_theft`        | A coin is taken from you and given to someone else (if possible). |
| `reveal_role`       | Broadcasts your name, role, and zone to the entire server.        |
| `dna_swap`          | Swaps role, HP, and inventory with a random non-SCP player.       |
| `hp_reduction`      | Reduces your health by 30%.                                       |
| `tp_to_class_d`     | Sends you back to the D-Class spawn.                              |
| `bad_effect`        | Random negative status effect.                                    |
| `warhead_toggle`    | Starts or cancels warhead.                                        |
| `lights_out`        | Turns off all facility lights.                                    |
| `live_he`           | Spawns live HE grenade with short fuse.                           |
| `troll_flash`       | Spawns live flashbang.                                            |
| `scp_tp`            | Teleports you to a random SCP.                                    |
| `one_hp`            | Sets your health to 1.                                            |
| `primed_vase`       | Gives you a dangerous cold vase.                                  |
| `tantrum`           | Forces tantrum effect.                                            |
| `fake_cassie`       | Fake CASSIE announcement.                                         |
| `random_scp`        | Transforms you into a random SCP.                                 |
| `inventory_reset`   | Clears your inventory.                                            |
| `class_swap`        | Changes your role to a counterpart.                               |
| `instant_explosion` | Instant HE grenade detonation.                                    |
| `player_swap`       | Swaps positions with another player.                              |
| `kick`              | Kicks the player from the server (if allowed).                    |
| `spectator_replace` | Swaps body with a spectator.                                      |
| `tesla_tp`          | Teleports player to Tesla zone.                                   |
| `inventory_swap`    | Swaps inventories with a random player.                           |
| `handcuff`          | Handcuffs the player.                                             |
| `random_tp`         | Teleports to random room from config.                             |
| `infectious_touch`  | Placeholder for infection effect.                                 |
| `name_change`       | Temporarily changes your nickname.                                |

---

## 🔧 Config Additions

```yml
GlobalSettings:
  HealAlliesRadius: 10
  HealAlliesAmount: 35
  DoorEffectRadius: 15
  # Time delays are baked into effect logic (e.g. lock = 30s, unlock = 5s)

GoodEvents:
  CoinTheftChance: 6
  HealAlliesChance: 15
  DoorEffectChance: 12
  ScpSummonChance: 5
  # ... other chances remain

BadEvents:
  CoinTheftChance: 5
  RevealRoleChance: 10
  DnaSwapChance: 4
  # ... other chances remain
```

---

## ⚠️ Notes

* **Anti-Exploit:** Checks if players are alive, not SCP, and not spectators for swap logic.
* **Sound Failover:** If audio files are missing, plugin logs a warning.
* **Dependencies:**

  * `AudioPlayerApi.dll` must be referenced and properly loaded.

---

## 🛠️ Upcoming / Skipped

* `Flip Trap` and `Clone Yourself` were intentionally skipped.
* Additional effects may be added via config without core changes.

---

## 🔄 Legacy Core Logic Overview

* Weighted random effect logic using total chance sums.
* Uses `HintManager` for UI feedback.
* Uses `Timing.CallDelayed` to manage audio cleanup and delayed logic.
* Multiple fallbacks and no-crash paths for all critical logic.
* `EffectHandler.cs` orchestrates all result logic.

---

## 🧪 Testing Checklist

* Flip coin -> check audio
* Flip coin -> verify cooldown or max use
* Flip coin -> get Good/Bad outcome, validate hint/effect
* Flip coin -> check logs if debug enabled
* Audio -> confirm correct file used and destroyed
* All effects -> validate config weight controls probability

---


---

## 🧑‍💻 Author

**LaFesta1749**

---

## 📜 License

MIT – Do whatever you want, just don't remove credits.

---

## 🛠 Built With

* [Exiled](https://github.com/ExSLMod-Team/EXILED/)
* [HintServiceMeow](https://github.com/MeowServer/HintServiceMeow)
* [AudioPlayerApi](https://github.com/Killers0992/AudioPlayerApi)
