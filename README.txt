Summary:
The goal of this project is to create a "chore game" that my wife and I enjoy and feel motivated to play. As such, I dedicate this project to her.
The premise is that for each room of the house, there is a list of chores worth a certain amount of points. 
Whoever gets the most points after a week gets a treat. 

Set Up:
1. Install Visual studio 2022
2. Create a four AWS Dynamo DB tables with the following layout - https://aws.amazon.com/dynamodb/getting-started/
  a. Users - TableName{Partition key}, UserID{Sort key}
  b. Rooms - TableName{Partition key}, RoomID{Sort key}
  c. Missions - TableName{Partition key}, MissionID{Sort key}
  d. Points - TableName{Partition key}, PointsID{Sort key}
  e. Logs - TableName{Partition key}, PointsID{LogID key}

3. Generate an AWS access Key and secret key - https://aws.amazon.com/blogs/security/how-to-find-update-access-keys-password-mfa-aws-management-console/
4. Install project and plug access key and secret key into MauiProgram.cs file
5. Load to device.
