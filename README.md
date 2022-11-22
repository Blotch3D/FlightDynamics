
FlightDynamics
==============

Let's say we want to know, as accurately as possible, the various forces, torques, surface temperatures, etc. on an aircraft under a given set of conditions.

The input conditions would be things like the power plant outputs, the control surface positions, propeller pitches, landing gear position, mass of each airframe part, relative air velocity on each airframe part, air density, ambient and current surface temperatures, etc. The outputs would be the force, torque, and surface temperatures on parts of the airframe or on the whole airframe.

This is a complex problem and typically done with CFD and/or by gathering empirical data.

But let's also say we want to accurately calculate the forces given ANY set of input conditions in near real-time, or at least notably faster than a CFD.

To simplify the problem, we'll also say it is OK to ignore or separately handle discontinuous effects like hysteresis between turbulent and laminar flow over smooth surfaces. We can also make sure we choose input conditions and output parameters such that their relationships are all continuous, so that a mathematical solution is at least possible.

If we do these things, then the best way to quickly and accurately calculate forces from any input conditions is to keep a limited database of known cases previously gathered empirically or from CFD models, and then interpolate/extrapolate the new cases from them as they appear. That way we will have both high accuracy and fast results (assuming the pre-defined cases are numerous enough and/or well chosen).

Still, the number of pre-defined cases is substantial to produce fairly good accuracy when interpolating. So, the database of pre-calculated cases should be created intelligently so that we aren't spending weeks or months creating many more cases than are needed.

Each input condition can be thought of as a dimension in a space within which we do our interpolation. We want to intelligently pick in that space the positions to pre-calculate, rather than simply calculating them for each position in a regular grid. Specifically, for a given region in that space if any output is experiencing a notably higher order relationship (that is, the function of that input to any output is high order and magnitude), then cases should be dense in that area through that dimension. Likewise cases can be sparse where no output experiences a notably higher order relationship in any dimension. Obviously, since thousands of CFD models will need to be created, this decision-making process should be automated.

Further optimization can be had by accounting for airframe symmetry, choosing the airframe parts wisely, performing a hybrid process of mixing RBF interpolation with estimator equations, etc.

The bottom line is that interpolation requires a lot of well chosen initial cases (many thousands, possibly taking weeks to create), but then quickly produces high accuracy results.

Look at it this way: If in the past you've been taking man-weeks to tweak equations to produce a fast but imperfect calculation of forces on the fly, how is that better than simply letting a computer pre-calculate several thousand good CFDs for a few weeks, and then getting both fast and accurate results from then on?

Interpolation of non-regular data points in higher dimensions can be done with a radial basis function (RBF) interpolation. And that's what FlightDynamics does (by using the alglib math library). See https://en.wikipedia.org/wiki/Radial_basis_function_interpolation and https://www.alglib.net/interpolation/fastrbf.php for more information.

Using FlightDynamics:

All input condition values must be scaled between 0 and 1. For example, an elevator value of 0 means all the way down, and 1 means all the way up. other than that, the number and meaning of each input and output force, etc is whatever you like. You can give names to each, but the program doesn't care about them.

When you start FlightDynamics, you must pass the database of known cases in CSV format to the program with the '-db:' command line option. For example:

FlightDynamics -db:MyDbFile.txt

FlightDynamics takes a while to compile that database. Then, each time you enter (via stdin) a line of comma-separated input conditions in the same order they appear in a database records, FlightDynamics will return a line of the output values in the same order as the outputs used by the database records.

A new input line is just each input value separated by a comma like this, for example:
.215, -.101, 0.026, -.593, 0, .2, 0, .5, 0, 0

So, you can automate the interaction with FlightDynamics by programmatically sending and receiving lines through stdin and stout. (Although the programmatic interface is just as easy and quicker if you know a little C#)

The database file has the following format:

The first line of the file is a list of input condition names separated with commas in the order they will be used. FlightDynamics uses the number to define the number of inputs, but cares nothing about the names.

The second line is a list of output force names separated with commas in the order they will be used. FlightDynamics uses the number to define the number of outputs, but cares nothing about the names.

The third line is a couple of RBF parameters separated by a comma: The Radius and NLayers. until you understand these well, just use zero for both, which indicates use a default.

Database records start on the fourth line, one record per line. Records need not be in any order. A record is simply the input values followed by output values.

Whitespace in a line and blank lines are ignored.

On any line, all text after a '#' character is a comment.

See database.txt for an example.
