# MarsRoverChallenge

Please follow these instructions to build the Mars Rover Challenge solutions:
1. Download the files.
2. Extract the zip folder.
3. Open the "RoverChallenge.sln" file in Visual Studio.
4. In the IDE, select the Run button. The application will build and run.

Upon startup you are presented with the following screen, please do the following:
1. Choose your rover count.
2. Select the plateau width.
3. Select the plateau height.
4. Press the next button.

In this example, we will be using 5 rovers, with a plateau width and height of 10.

![image](https://github.com/tinusjvr/MarsRoverChallenge/assets/68852823/3c462f4d-9670-4bf3-9e3a-18567f979cf4)

After pressing next, you will be shown the configuration screen, here you need to input starting positions for a rover and its instructions.
The 5 rovers used will have the following starting positions (x,y) and instructions (N,E,S or W):

1. Rover 1
   - (0,0)
   - NENENENENE
2. Rover 2
   - (2,3)
   - NNNEESSS
3. Rover 3
   - (5,6)
   - NNWWWWSSEN
4. Rover 4
   - (7,3)
   - WWWWWWW
5. Rover 5
   - (4,7)
   - WSSWWSS

The following image shows the configuration screen. Please note the following:
- The rovers input is provided via a datagrid.
- Please do not enter values under the "X-Current" and "Y-Current" columns as no exception handling is built in yet.
- The instructions per rover can be set using the "Instructions" column.
- After inputting the required data, you can press the "Get Results" button, which will replace the instructions shown, with the results screen.

![image](https://github.com/tinusjvr/MarsRoverChallenge/assets/68852823/04e37708-7de6-4192-b298-99f88428f1bf)

After pressing the "Get Results" button, the results window is still blank. Please select either the "Map" or "Intersections" button.

When selecting the intersections button, the following will be shown in the results window:
![image](https://github.com/tinusjvr/MarsRoverChallenge/assets/68852823/71be5316-6b4c-4cb2-9f43-52e13365b986)

In the image above, it can be seen that the 5 rovers, with their instructions, will cross paths with each other 9 times. 
When selecting the "Map" button, the following will apeear:
![image](https://github.com/tinusjvr/MarsRoverChallenge/assets/68852823/1a4b96d4-5cae-4ee5-80de-550a013884d5)

Now, select either of the buttons shown below the title. Selecting the "Starting Positions" button will show the following:
![image](https://github.com/tinusjvr/MarsRoverChallenge/assets/68852823/e8446c12-d524-43a7-9dfd-90a4849a4a9c)

Selecting "Ending Positions" will show the following:
![image](https://github.com/tinusjvr/MarsRoverChallenge/assets/68852823/5c62efe7-653b-48e1-8739-4b3f8cc01a14)
Please note that the ending positions are also shown in the datagrid.

Lastly, selecting "Intersections" will show the following:
![image](https://github.com/tinusjvr/MarsRoverChallenge/assets/68852823/ae6259fc-83b6-4353-839e-9428ec3b6cba)

Selecting "Restart" will clear all the data pertaining to the rovers and the resulting intersections and maps.
This is shown below:
![image](https://github.com/tinusjvr/MarsRoverChallenge/assets/68852823/d526a6cb-97e1-4538-bfbb-c88648f13e85)

Selecting back will return to the rover and plateau configuration screen.



