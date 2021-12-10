Something You Must Know About "Space" Parameter in Shader

Take a look at the Viedo First: https://youtu.be/vLTrRjugr-U

-------------------------------------------

There are 3 types of "Space" you can choose.
* Local Space
* World Space
* World Triplanar Space

-------------------------------------------

Local Space

Sample texture in Object Local Space.

Demo1: Assets/Stylized Ice/Demo/Demo1.unity

-------------------------------------------

World Space

Sample texture in World Space.

Useful for overlapped planes.

If you choose "Local Space" for overlapped planes, you will see flickers at overlapped area.
And if you rotate planes, edge texture of planes are not continuous any more.
We choose "World Space" to resolve these problems.

Demo2: Assets/Stylized Ice/Demo/Demo2.unity 

-------------------------------------------

Wrold Triplanar Space

Add Triplanar UVs base on "World Space". Triplanar UVs make texture sampling at large slope with less stretch.

Demo3: Assets/Stylized Ice/Demo/Demo3.unity 

-------------------------------------------

Script: StylizedIce_Plane_InWorldSpace.cs

Add this script to gameobject If you are using "World Space" or "World Triplanar Space".

Parameter of this script: tangentBaseCorrection
Tangent directions of meshes maybe different. 
Using this parameter to add a rotation to tangent to make tangent directions the same.

Take a look at Demo2 and Demo3 for more details.

-------------------------------------------

If you still see some flickers at overlapped area or edge texture of planes are not continuous, 
Please make sure XYZ-axes directions of object local space are correct. 
X-axis point to right, Y-axis point to up, Z-axis point to forward.
Using your authoring tools(Maya, 3dMax or Blender ......) to modify axes directions.