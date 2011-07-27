using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorldGeneration
{
    public class MyPerlin
    {
        public MyPerlin() { }

        private int seed = 0;
        private Random rand;

        private int[,] grad3 = {{1,1,0},{-1,1,0},{1,-1,0},{-1,-1,0},
                                       {1,0,1},{-1,0,1},{1,0,-1},{-1,0,-1},
                                       {0,1,1},{0,-1,1},{0,1,-1},{0,-1,-1}};

        public void setSeed(int s)
        {
            seed = s;
            rand = new Random(seed);
        }

        private double noise(double x, double y, double z)
        {
            int xi = fastfloor(x);
            int yi = fastfloor(y);
            int zi = fastfloor(z);
            return (xi + yi * seed + zi * seed * seed)%255;
            /*
            if (n <= 0)
            {
                return 0.0f;
            }
            else
            {
                return (double) rand.Next(n) / n;
            }*/
        }


        private int smooth(double x, double y, double z)
        {
            
            double corners = (noise(x - 1, y - 1, z - 1) + noise(x + 1, y - 1, z - 1) + noise(x - 1, y + 1, z - 1) + noise(x + 1, y + 1, z - 1) +
                             noise(x - 1, y - 1, z + 1) + noise(x + 1, y - 1, z + 1) + noise(x - 1, y + 1, z + 1) + noise(x + 1, y + 1, z + 1)) / 32;

            double sides = (noise(x - 1, y, z) + noise(x + 1, y, z) + noise(x, y - 1, z) + noise(x, y + 1, z) +
                           noise(x, y, z - 1) + noise(x, y, z + 1)) / 16;

            double center = noise(x, y, z) / 8;

            return (int) (corners + sides + center);
        }

        // This method is a *lot* faster than using (int)Math.floor(x)
        private int fastfloor(double x)
        {
            return x > 0 ? (int)x : (int)x - 1;
        }

        private double dot(int g, double x, double y, double z)
        {
            return grad3[g, 0] * x + grad3[g, 1] * y + grad3[g, 2] * z;
        }

        public double cosInterp(double a, double b, double x)
        {
            double ft = x * Math.PI;
            double fl = (1f - Math.Cos((Double)ft)) * 0.5f;
            return a * (1f - fl) + b * fl;
        }

        private double fade(double t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        // Classic Perlin noise, 3D version
        public double intNoise(double x, double y, double z)
        {
            // Find unit grid cell containing point
            int xi = fastfloor(x);
            int yi = fastfloor(y);
            int zi = fastfloor(z);

            // Get relative xyz coordinates of point within that cell 
            x = x - xi;
            y = y - yi;
            z = z - zi;

            // Calculate a set of eight hashed gradient indices
            int n1 = smooth(xi, yi, zi) % 12;
            int n2 = smooth(xi + 1, yi, zi) % 12;
            int n3 = smooth(xi, yi + 1, zi) % 12;
            int n4 = smooth(xi + 1, yi + 1, zi) % 12 ;
            int n5 = smooth(xi, yi, zi + 1) % 12;
            int n6 = smooth(xi + 1, yi, zi + 1) % 12;
            int n7 = smooth(xi, yi + 1, zi + 1) % 12;
            int n8 = smooth(xi + 1, yi + 1, zi + 1) % 12;


            // The gradients of each corner are now:
            // g000 = grad3[gi000];
            // g001 = grad3[gi001];
            // g010 = grad3[gi010];
            // g011 = grad3[gi011];
            // g100 = grad3[gi100];
            // g101 = grad3[gi101];
            // g110 = grad3[gi110];
            // g111 = grad3[gi111];

            // Calculate noise contributions from each of the eight corners
            double n000 = dot(n1, x, y, z);
            double n100 = dot(n2, x - 1, y, z);
            double n010 = dot(n3, x, y - 1, z);
            double n110 = dot(n4, x - 1, y - 1, z);
            double n001 = dot(n5, x, y, z - 1);
            double n101 = dot(n6, x - 1, y, z - 1);
            double n011 = dot(n7, x, y - 1, z - 1);
            double n111 = dot(n8, x - 1, y - 1, z - 1);

            // Compute the fade curve value for each of x, y, z
            double u = fade(x);
            double v = fade(y);
            double w = fade(z);

            // Interpolate along x the contributions from each of the corners
            double nx00 = cosInterp(n000, n100, u);
            double nx01 = cosInterp(n001, n101, u);
            double nx10 = cosInterp(n010, n110, u);
            double nx11 = cosInterp(n011, n111, u);

            // Interpolate the four results along y
            double nxy0 = cosInterp(nx00, nx10, v);
            double nxy1 = cosInterp(nx01, nx11, v);

            // Interpolate the two last results along z
            double nxyz = cosInterp(nxy0, nxy1, w);
            return nxyz;
        }

        public double perlin(double x, double y, double z, int octaves, double freq, double amp, double per)
        {
            double total = 0.0f;

            for (int i = 0; i < octaves; ++i)
            {
                total = total + (intNoise(x * freq, y * freq, z * freq) * amp);
                freq *= 2.0f;
                amp *= per;
            }

            return total;
        }
    }
}
