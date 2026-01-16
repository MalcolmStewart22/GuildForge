# Current Design Goals — GuildForge
## Project Overview

GuildForge is a simulation-driven guild management game that emphasizes long-term decision making and indirect player control. 
Gameplay centers on assigning characters to missions and advancing time, with outcomes resolved through underlying systems rather than moment-to-moment input.

The core design goal is to explore emergent narrative design through the interaction of discrete-event simulation and procedural generation, where player decisions shape long-term story outcomes rather than scripted sequences.

GuildForge is intentionally developed as a systems-first prototype, prioritizing clarity, inspectability, and iteration over content polish. 
Features are evaluated based on their contribution to meaningful, system-driven storytelling.

## Core Design Pillars
**Emergent Narrative** - Narrative outcomes are not scripted. Stories emerge from character traits, mission outcomes, and accumulated world state rather than predefined plotlines.

**Simulation-Driven Consequences** - Time progression, mission resolution, and world changes are governed by a discrete-event simulation model, enabling delayed and cascading narrative effects rather than immediate feedback.

**Procedural Story Inputs** - Characters, missions, and locations are procedurally generated to provide varied narrative building blocks within a consistent and understandable rule set.

**Persistence and Memory** - The world maintains memory of past events. Outcomes influence future possibilities, allowing narrative context to accumulate organically over time.

**Inspectable Systems** - Narrative outcomes must remain traceable to underlying systems to support iteration, debugging, and refinement of emergent behavior.

## MVP (Current Focus)
The MVP represents a minimal but complete vertical slice demonstrating how emergent narrative can arise from simulation and procedural systems.

### Currently Implemented
- Procedural generation of characters and dungeons driven by tag-based parameters
- Discrete-event simulation controlling time and mission resolution
- Core gameplay loop for assigning missions and advancing time
- Persistent world state updated through simulation events
- Functional character roles defining baseline capabilities and mission participation

### In Progress
- Finalizing after-action report generation
- Implementing end-of-game statistics and summary views
- Party recruitment and mission assignment logic
- Character and Guild Rank progression systems
- Adjusting systems to add:
    - Dungeon Fleeing
    - Selective Event Resolution 

The MVP is designed to be functional, inspectable, and demonstrable without relying on authored content or visual polish.

## Full Demo (Next Milestone)
Once MVP systems are stable, the next milestone focuses on increasing narrative richness while preserving the same architectural foundations.

### Planned Expansions
- Event-driven dungeon evolution where locations change in response to narrative and simulation events
- Player-directed party composition
- Dynamic character traits that evolve over time based on mission outcomes and character experiences
- Distinct character classes that define capabilities and influence mission outcomes within the simulation

These features are intended to deepen emergent narrative without introducing new core paradigms.

## Future Exploration(Conceptual)
- Equipment and item systems that modify character capabilities and introduce additional strategic constraints
- Character memory systems that record significant experiences and influence future behavior
- Rival guilds operating within the simulation, creating external pressure and competing narratives
- Persistent world continuity across multiple playthroughs, allowing prior game history to influence future runs

