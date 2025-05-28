# Mesh Renderer

This project is a 3D Mesh Renderer developed in C#, featuring a Bezier surface representation, custom rendering pipeline, and support for phong shading, textures and normal mapping. It allows users to manipulate 3D scene, including applying rotation and controlling lighting parameters.

## Features

- **Bezier Surface Representation**: Load, render, and interact with 3D Bezier surfaces defined by a 4x4 control point grid.

- **Dynamic Mesh Generation**: Create high-quality meshes from Bezier surfaces with adjustable density.

- **Custom Rendering Pipeline**: A specialized rendering system that includes support for both wireframe and solid shading modes, using Z-buffer.

- **Phong Shading Model**: Realistic lighting effects are implemented through Phong shading, which simulates how light interacts with surfaces, including diffuse, and specular reflections.

- **Texture and Normal Mapping**: Enhance the visual fidelity of 3D models by applying textures and normal maps. Textures allow for detailed surface appearance, while normal maps add depth and realism by simulating surface irregularities.

- **Interactive GUI**: The application provides a responsive GUI for real-time adjustments to 3D models and rendering parameters. Users can toggle wireframes, change textures and normal maps, and adjust lighting directly through the interface.

- **Real-time Transformations**: Transform 3D scene in real-time by applying rotation.

- **Animation Control**: Built-in animation functionality enables dynamic light movement.

- **Light Source Manipulation**: Adjust light properties, including position, color, and intensity, to achieve the desired visual effects. Lighting can be controlled both statically and dynamically, allowing for simulations of changing lighting conditions.

## Getting Started

### Prerequisites

- .NET 6.0 SDK or higher
- Visual Studio or another C# IDE

### Installation

1. Clone the repository:

   ```
   git clone https://github.com/F1r3D3v/MeshRenderer.git
   ```

2. Open the solution in your IDE.

3. Build the project.

> [!NOTE]  
> Compile in Release for better performance.

### Running the Application

1. Ensure the resources folder contains any required assets like control points files for Bezier surfaces.

2. Run the application from your IDE. The renderer window will launch, allowing you to interact with 3D scene, adjusting lighting parameters, and applying rotation.
