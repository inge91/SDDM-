using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Text.RegularExpressions;

// a 'dirty code' class only used to parse printed direction results and recalculate the angles
public class ResultConverter 
{

	private string directory = "ConvertedTestResults/";
		System.IO.StreamWriter outputfile;
        System.IO.StreamReader inputfile;

		public ResultConverter (string inputfilename)
		{
			// Find filename in directory and add number to distinguish between already existing files.
			DirectoryInfo logDirectory = new DirectoryInfo(directory);
			FileInfo[] taskFiles = logDirectory.GetFiles (inputfilename  + ".csv");
			if(taskFiles.Length != 1)
			{
					throw new System.Exception("file not found");
			}
            else
            {
                inputfile = new StreamReader(directory + inputfilename + ".csv");
			    outputfile = new System.IO.StreamWriter (directory + inputfilename + "FIXED" + ".csv");
			    outputfile.WriteLine ("Iteration number; Actual position; Estimated postion; orinal Angle; new angle; Horizontal angle; Vertical angle; Time");

                //skip first 2 lines
                string line = inputfile.ReadLine();
                if(line != "Iteration number; Actual position; Estimated postion; Angle; Horizontal angle; Vertical angle; Time")
                    throw new System.Exception("wrong header");
                inputfile.ReadLine();


                while((line = inputfile.ReadLine()) != null && line != "--------Experiment Done------------------")
                {
                    string[]pieces = line.Split(';');
                    if (pieces.Length != 7)
                        throw new System.Exception("wrong input length");
                    string output = pieces[0] + "; " + pieces[1]+ "; " + pieces[2] +"; " + pieces[3] + "; " ;
                    // parse vectors
                    string actualvector = pieces[1];
                    string estimatedvector = pieces[2];
                    Vector3 actual = stringToVector(actualvector);
                    Vector3 estimated = stringToVector(estimatedvector);
                    output += convert(actual, estimated);                   
                    output += pieces[6];
                    outputfile.WriteLine(output);
                }
                Close();
               
            }
		}

        public Vector3 stringToVector(string input)
        {
            input = Regex.Replace(input, @"\s+", ""); //remove spaces etc
            input = Regex.Replace(input, "[()]", ""); // parentheses
            string[] coords = input.Split(',');
            if (coords.Length != 3)
                throw new System.Exception("not a 3d vector");
            float x = float.Parse(coords[0]);
            float y = float.Parse(coords[1]);
            float z = float.Parse(coords[2]);

            return new Vector3(x, y, z);
        }

        public string convert(Vector3 actual, Vector3 estimate)
        {
            // shortest 3d angle
            float angle = Vector3.Angle(actual, estimate);

            //rotation around y axis (head left to right)
            double rad2Deg = (180.0 / System.Math.PI);
            float angle1 = (float)(System.Math.Atan2(actual.x, actual.z) * rad2Deg);
            float angle2 = (float)(System.Math.Atan2(estimate.x, estimate.z) * rad2Deg);
            float diff = Mathf.DeltaAngle(angle1, angle2);
            //move rotate points to be the same angle around y
            Vector3 actualR = Quaternion.Euler(0, diff, 0) * actual;

            float yaw = Mathf.Abs(diff);
            float pitch = Vector3.Angle(actualR, estimate);

            string output = angle + "; " + yaw + " ; " + pitch + "; ";
            return output;
        }


		~ResultConverter()
		{
			inputfile.Close ();
            outputfile.Close();
		}

		public void writeLineToFile(string line)
		{
			outputfile.WriteLine (line);
		}

		public void Close()
		{
			inputfile.Close();
            outputfile.Close();
		}
	
}
