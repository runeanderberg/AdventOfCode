import java.io.*;
import java.util.*;

public class Day1 {
    public static void main(String[] args) throws IOException {
        long startTime = System.nanoTime();
        calculate();
        long elapsedTime = System.nanoTime() - startTime;
        System.out.println("execution time (ms): " + elapsedTime/1000000);
    }

    private static void calculate() throws IOException {
        var input = new File("inputs/day1.txt");

        var br = new BufferedReader(new FileReader(input));

        int read;
        int number = 0;
        int sum = 0;
        int[] highest = new int[3];

        while ((read = br.read()) != -1) {
            if (read == '\r') {
                br.read();
                sum += number;
                number = 0;

                if ((read = br.read()) == '\r') {
                    br.read();

                    if (sum > highest[highest.length - 1]) {
                        for (int i = 0; i < highest.length; ++i) {
                            if (sum > highest[i]) {
                                for (int j = highest.length - 1; j > i ; --j) {
                                    highest[j] = highest[j - 1];
                                }
                                highest[i] = sum;
                                break;
                            }
                        }
                    }

                    sum = 0;
                    continue;
                }
            }

            number *= 10;
            number += read - '0';
        }

        br.close();

        System.out.println(highest[0]);
        System.out.println(Arrays.stream(highest).sum());
    }
}