# Unity Slot Machine Game

## Game Overview

This project is a 3-reel slot machine game developed using Unity and C# as part of the Unity Slot Game Assignment.

The player can place bets, spin the reels, and try to match three identical symbols to win credits.

### Gameplay Features

- Three animated slot reels.
- Random symbol selection using Unity's random number generation.
- Four symbols: Seven, Cherry, Bell, and BAR.
- Smooth reel movement and staggered reel stopping.
- Starting balance of 1,000 credits.
- Adjustable bet from 10 to 100 credits.
- Winning combinations and symbol-based payouts.
- Win popup displaying the winning amount.
- Lever interaction with default and pulled visual states.
- Exit confirmation popup.
- Spin sound effect.
- Separate audio playback methods for win, lose, reel-stop, and button effects, where clips are assigned.

## Controls and Instructions

1. Start the game.
2. Use the left and right bet buttons to decrease or increase your bet.
3. Pull the slot machine lever or press the Spin button to start a spin.
4. Wait for all three reels to stop.
5. Match all three symbols to win credits.
6. Close the winning popup to continue playing.
7. Use the exit button to open the exit confirmation popup.

### Winning Rules

You win when all three reels display the same symbol.

The payout is determined by the selected symbol's payout value and the current bet.

## Running the WebGL Build

The WebGL build is included in:

`Build/WebGL/`

To run the game:

1. Download or clone this repository.
2. Open the `Build/WebGL` folder.
3. Run the WebGL build through a local web server or a supported web hosting service.
4. Open the provided web page in a compatible browser.

**Note:** WebGL builds should be served over HTTP or HTTPS. Opening `index.html` directly from File Explorer may not work correctly.

## Technologies Used

- Unity
- C#
- Unity UI
- TextMeshPro
- ScriptableObjects
- Unity Coroutines
- AudioSource

## Project Structure

```text
Assets/
├── Scripts/
├── Prefabs/
├── Animations/
├── UI/
├── Sounds/
├── ScriptableObjects/
└── Scenes/

Build/
└── WebGL/
```

## Development Approach

The project was developed using a modular approach.

- A ScriptableObject stores the slot symbol information and payout values.
- Each reel uses a reusable SlotReel component to generate and animate its symbols.
- A SlotGameManager controls the game flow, betting, random results, winning checks, and UI updates.
- A LeverAnimator controls the lever's visual states.
- A SlotAudioManager handles the game's sound effects.
- UI elements provide feedback about the player's balance, bet, results, and confirmation popups.

The focus was on creating a functional slot machine with reusable components, organized scripts, and clear separation of responsibilities.

## Author

Developed as part of the Unity Slot Game Assignment.