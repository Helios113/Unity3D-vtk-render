# vtkUnity

vtkUnity is a project which aims to deliver universal compatibility between vtk files and the Unity3D game engine.   
The project comprises of series of open source C# scripts which currently implement support for non-XML vtk files. The scripts are written with hackability and portability in mind. The main motivation for this project was to allow for the dynamic render of dynamic datasets on VR headsets which run android (Oculus Quest). This resulted in a library which is fully portable and can support all mobile, desktop and web platforms.

## Functionality
vtkUnity allows people to upload standard vtk files into the unity editor and render them as gameObjects.
### Portability
vtkUnity is a fully portable project and supports all devices covered by the unity engine using the same scripts. This is achieved in two ways.

Firstly, all scripts are written entirely in C#; hence they act as a managed unity plugin which is fully portable on all supported unity platforms.

Secondly, all vtk files are converted to unity text assets before they are parsed. This allows us to load them through the **Resource** folder which again guarantees functionality on all supported platforms.

These aspects make this project great for VR implementations, where the target device may be running android such as the Oculus Quest and indeed that was the main motivation behind this project.
### Vertex Coloring
The project supports custom materials, both regular and Universal render pipeline, with vertex coloring which is used to display various vector and scalar fields directly on the object's geometry, as shown below:

![Vertex coloring of 2D object](Renders/render1.png)

### Geometry wrapping
The project also supports vector wrapping for 3D objects and scalar wrapping for 2D objects. This can be used to as a visual aid to show things such as an object's displacements:

### In Editor tools
vtKUnity provides the user with editor tools to convert vtk files into unity text assets. This was done to ensure portability and smooth execution on various platforms, namely Android and iOS devices.

![In editor tools](Renders/render2.png)

### Animation
vtkUnity offers animation support to display dynamic datasets. It offers granular setting to change frame rate and the displayed vector/scalar field dynamically in-game, which allows users to create custom control panels for players.
![Loop animation](Renders/Animation.gif)



### Compactness