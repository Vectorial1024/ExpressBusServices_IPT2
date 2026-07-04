# ExpressBusServices_IPT2
Optional sister mod of Express Bus Services (https://github.com/Vectorial1024/ExpressBusServices) for better compatibility with IPT2: if also using IPT2, you can now also specify how this mod should interpret IPT2 bus unbunching instructions to be used in the insta-depart behavior of Express Bus Services.

Note: if using Improved Public Transport Essentials (IPTE, https://steamcommunity.com/workshop/filedetails/?id=3714961481 ) then DO NOT use this mod! Just use IPTE directly; no plugins required.

> [!NOTE]
> ARCHIVED on 4 July, 2026
>
> Because Improved Public Transport Essentials (IPTE, https://steamcommunity.com/workshop/filedetails/?id=3714961481 ) has integrated this plugin into their source code, and IPTE is the continuation of IPT2, this repo is now archived. Please redirect support questions to IPTE, though I may sometimes also respond at IPTE.

## Unbunching interpretation modes
This mod offers a total of 3 unbunching interpretation modes to interpret unbunching instructions from IPT2.

### Option 1: First Principles
When a bus is about to depart:
1. If ths stop that the bus is at is not the first stop of the bus line, then it is OK to depart (waits for other safety checks)
2. Else, unbunch

Note: this is essentially what the base mod is doing to determine instant departure.

### Option 2: Respect IPT2 unbunching
When a bus is about to depart:
1. If IPT2 says that the bus should not unbunch at the stop it is at, then it is OK to depart (waits for other safety checks)
2. Else, unbunch

### Option 3: Invert IPT2 unbunching
When a bus is about to depart:
1. If IPT2 says that the bus should unbunch at the stop it is at, then it is OK to depart (waits for other safety checks)
2. Else, unbunch

Note: this option is designed to quickly deploy "unbunch buses only at termini/designated stops" configuration with IPT2 by going through this process:
1. Make a new bus line; by IPT2's default, all stops in the line have unbunching = YES
2. Pick the bus termini/designated stops
3. For those stops, set unbunching = NO
4. Switch interpretation to "Invert IPT2 unbunching"
5. Setup complete
   1. Verify that the buses now unbunch at those termini/designated stops
   2. Verify that the buses can depart quickly at all other stops
