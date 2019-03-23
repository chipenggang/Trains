## Introduction
The purpose of this program is to help the railroad provide its customers with information about the routes and compute the distance between the two towns , and compute the shortest route. this program build with .net core 2.2, and used with DDD(Domain-Driven Design). Because it takes times to build a map, the singleton pattern is used for the map service. The specific design is as follows:

#### 1.Data load
The way to get the map station information is by load the physical file, file name and file path is configurate in appsettings.json, and the setting of 'MapFileInfo.FileName' is necessary, but the path is not, the path meaning physical file's path int computer, default value is executable file directory. The data format of map must be statisfied with 'AA1', and use ',' to split.

#### 2.Map build
Create domain for every station, and the station is unique in map for same station name, and use the line to relate station each, so the map like a nets.

- station domain: station information contains name and line list from this station, and the station domain can compute the distance to next station.
- line domain：line domain contains the next station information  and the distance.

#### 3.Compute the distance
Compute the distance according to line just need to find the next station node, and then accumulate the distance, if the next station is null then throw a message named 'NO SUCH ROUTE'.

#### 4.Compute route numbers and shortest route
I used a similar way to solve for these qustion：Breadth-First Search. I used a queue to search the map, and make diffrent validator for diffrent situation.
