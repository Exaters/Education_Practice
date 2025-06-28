# Education_Practice
Creating a application to solve a Monte-Carlo problem, for education practice.
This program is designed to calculate the area of ​​a larger segment of a circle cut off by a vertical or horizontal line using the Monte Carlo method.
This statistical approach allows you to get an approximate area value by generating random points and analyzing their occurrence in a given area.

The program provides the user with an intuitive interface for setting the parameters of the circle
(coordinates of the center, radius), selecting the position of the secant line (vertical or horizontal) and specifying its offset relative to the center. 
The accuracy of the calculations is regulated by the number of generated points, which allows you to balance between the speed of work and the accuracy of the result.

The key features of the program include:
	Visualization of the circle, secant line and distribution of random points
	Automatic saving of calculation results to the SQLite database
	Building histograms to analyze the dependence of the accuracy of calculations on the number of points
	Export data for further processing

The program can be useful for educational purposes to demonstrate the Monte Carlo method, 
as well as in engineering and scientific calculations where it is necessary to estimate the areas of complex geometric figures. 
Ease of use is combined with flexible settings, which makes the program accessible to both beginners and experienced users.