# Virtual Reality Solar System using Unity
 This repository holds the 3rd Year Physics Project of [Bruhmao](https://github.com/Bruhmao "My partner's GitHub Profile") and myself, containing development history and the final submitted code.

 By the time of presentation, the Unity `Assets` folder should contain all that is required to load up the project using the Unity Editor. On top of this, there should also be a `Builds` folder containing the executable files that allow the simulation to be run independently of Unity. There is a build for non-VR use, with less of the intended features as this was used for development, and another to be loaded using a connected VR device.
 ***
 ## Project Overview
 Using Newtonian Gravity and scaling methods, a model Solar System was built inside the Unity Engine so that the orbits of celestial bodies could be simulated. Starting from initially set conditions, the orbits evolve naturally with time and can easily be disrupted by changing properties of the celestial or by placing another massive body beside it (see below).

 Scripts required for any scene to work are:
- `SimulationScript.cs`
- `BodyProperties.cs`
- `PlanetRingMeshGenerator.cs`
- `UpdateTimeScale.cs`

 ### Non-VR Features
 Both simulators feature a method to explore the Solar System model through free movement or focused perspectives of a celestial. In the non-VR simulator, the user can use the `WASD` keys + `RMB` input to move freely in the 'Free Cam' mode. Alternatively they can use the 'Focus Cam' mode to bring a celestial body into view via UI or switching through the list by using the `Ctrl` + `<` or `>` keys.

 Scripts specific to this simulator version are:
- `Camera/CameraFocus.cs`
- `Camera/FreeCam.cs`
- `CelestialSelector.cs`
- `DistanceDisplay.cs`
- `PlanetProperties.cs`
- `KeyPadScript.cs`


 ### VR Specific Features
 In the scene migration to a VR compatible scene, the 'Free Cam' mode was replaced with the use of an XR Rig which the user takes control of via the Oculus Rift CV1 Device. Movement is controller using the `left joystick` whilst rotation can be controlled using the `right joystick`, note that this is relative to the direction you are looking. Speed is decreased with the `left trigger` and increased with the `right trigger`. Interactable objects are grabbed the usual way, pressing the `grip` button on either controller. The User Interface is interacted via raycast interactions using the right controller and selected using the `primary button`, which is the `A` button on the Oculus controller.

 Scripts specific to the VR version are:
- `VR/SphereGrabbableSpawner.cs`
- `VR/VRCamSwitch.cs`
- `VR/VRCelestialSelector.cs`
- `VR/ContinuousMovement.cs`
- `VR/DistanceDisplay.cs`
- `VR/PlanetProperties.cs`
- `VR/VRKeypadScript.cs`

***
### External Resources
Linked below will be a YouTube playlist showcasing development and previews of the project and any particular sources of information used in the project

YouTube Playlist: [VRSS Project Update Playlist](https://youtube.com/playlist?list=PLJfkNK5Lym90kyMIWkTr8NJP-mRjM5rE3)

Excel Workbook: [Planetary Datasheet (Read Only)](https://1drv.ms/x/s!AmE2NJQPF5Ygis8KAuAMAZCVfMHBrQ?e=OdOh1F)

#### Relevant Tutorials
Wherever possible, we try to cite tutorials/sources used when creating or taking from (open source) scripts. Below are links/references used in creating the project.

#### General Tutorials
Coderious's Solar System Example: [Unity Tutorial - Solar System with Unity Physics](https://www.youtube.com/watch?v=kUXskc76ud8)
> This was used to template custom gravity and velocity calculations in `SimulationScript.cs`. Due to incompleteness in the video, a unique solution was implemented to initialise starting velocities, and hence dependency on `BodyProperties.cs`.

Board To Bits Games Planet Ring Mesh Tutorial: || [Part 1](https://www.youtube.com/watch?v=Rze4GEFrYYs) || [Part 2](https://www.youtube.com/watch?v=WmWMC6iq7Y0&t=46s) ||
> Used to create planetary rings in the simulation for visual effects, see `PlanetRingMeshGenerator.cs`.

Emma Prats' Camera Rotation Tutorial: || [Video Tutorial](https://www.youtube.com/watch?v=rDJOilo4Xrg) || [Blog Tutorial](https://emmaprats.com/p/how-to-rotate-the-camera-around-an-object-in-unity3d/) ||
> Used to allow camera rotation around a celestial body in 'Focus Mode' for the non-VR scene.

Jason Wiemann's Spawning Tutorial: [Unity3D 101: Spawning Objects from prefabs...](https://www.youtube.com/watch?v=9KOHclqSmR4)
> Used to script `VR/SphereGrabbableSpawner.cs` to allow creation of VR interactable celestials.

Brackey's Minimap Tutorial: [How to make a Minimap in Unity](https://www.youtube.com/watch?v=28JTTXqMvOU&t=8s)
> This was used during scene migration to VR, allowing one camera to render to the UI allowing the user to focus on a celestial body whilst flying through the simulation.
>
#### VR Tutorials
Valem's Unity XR Toolkit Tutorial Playlist: [Introduction to VR in Unity - UNITY XR TOOLKIT](https://youtube.com/playlist?list=PLrk7hDwk64-a_gf7mBBduQb3PEBYnG4fU)
> This was most used during VR implementation, specifically parts 1 to 6.

Valem's Oculus Integration Tutorial Playlist: [How to make a VR game - OCULUS INTEGRATION](https://youtube.com/playlist?list=PLrk7hDwk64-Y7ELKfkw8ox8TaT9y3gNpS)
> This was used to help with using Oculus Integration Assets provided by Oculus themselves.

#### UI Tutorials
Jayanam's UI Dropdown Tutorial: [Unity UI Tutorial Dropdown C# Scripting](https://www.youtube.com/watch?v=URS9A4V_yLc)
> This tutorial was used to aid the creation of the Celestial Selector menu seen in the top left of the UI.

Coco Code's UI Scaling Tutorial: [How to scale Unity UI objects for every screen - Unity UI tutorial](https://www.youtube.com/watch?v=QnT-2KxVvyk)
> This was used to allow basic support for different screen sizes, so that UI would correctly display on-screen regardless of window size.