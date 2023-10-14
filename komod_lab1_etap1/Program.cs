using System;

public class LegendreApproximation
{
    public static double Function(double x)
    {
        return (x * x - 5 * x + 6) / (x * x + 1);
    }

    public static double FunctionT(double t)
    {
        double x = (9 * t + 1) / 2.0;
        return Function(x);
    }

    public static double Phi(int i, double t)
    {
        switch (i)
        {
            case 0: return 1;
            case 1: return t;
            case 2: return 0.4 * (3 * t * t - 1);
            case 3: return 0.285714 * (5 * t * t * t - 3 * t);
            case 4: return 0.222222 * (35 * t * t * t * t - 30 * t * t + 3);
            default: throw new ArgumentOutOfRangeException();
        }
    }

    public static double Integrate(Func<double, double> function, double a, double b, int n)
    {
        double h = (b - a) / n;
        double integral = 0.5 * (function(a) + function(b));

        for (int i = 1; i < n; i++)
        {
            integral += function(a + i * h);
        }

        return integral * h;
    }

    public static double Ci(int i)
    {
        double norm = 2.0 / (2 * i + 1);
        Func<double, double> integrand = t => FunctionT(t) * Phi(i, t);

        return norm * Integrate(integrand, -1, 1, 1000);
    }

    public static double LegendrePolynomial(double t)
    {
        double result = 0.0;
        for (int i = 0; i <= 4; i++)
        {
            result += Ci(i) * Phi(i, t);
        }
        return result;
    }

    public static void Main()
    {
        double[] coefficients = new double[5];
        for (int i = 0; i <= 4; i++)
        {
            coefficients[i] = Ci(i);
            Console.WriteLine($"c_{i} = {coefficients[i]}");
        }

        // Test
        double[] testValues = { -1, -0.5, 0, 0.5, 1 };
        foreach (double t in testValues)
        {
            double approximatedValue = LegendrePolynomial(t);
            Console.WriteLine($"P(t={t}) = {approximatedValue}");
        }
    }
}