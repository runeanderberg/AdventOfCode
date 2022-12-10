import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;

public class Day10 {
    public static void main(String[] args) throws IOException {
        long elapsedTimeSum = 0;
        for (int i = 0; i < 1000; i++) {
            long startTime = System.nanoTime();
            calculate();
            long elapsedTime = System.nanoTime() - startTime;
            elapsedTimeSum += elapsedTime;
        }
        System.out.println("average execution time (ns): " + elapsedTimeSum/1000);
    }

    private static void calculate() throws IOException {
        var input = new java.io.File("inputs/day10.txt");
        var br = new BufferedReader(new FileReader(input));

        var cpu = new Computer();
        String line;

        while ((line = br.readLine()) != null) {
            if (line.charAt(0) == 'n') {
                cpu.noop();
            } else {
                cpu.addx(Integer.parseInt(line.substring(5)));
            }
        }

        cpu.printSum();
    }


}

class Computer {
    private int cycleCounter;
    private int signalStrengthSum;
    private int registerValue = 1;

    void noop() {
        cycle();
    }

    void addx(int v) {
        cycle();
        cycle();
        registerValue += v;
    }

    void printSum() {
        System.out.println(signalStrengthSum);
    }

    private void cycle() {
        int pixel = cycleCounter % 40;

        if (pixel == registerValue - 1 || pixel == registerValue || pixel == registerValue + 1) {
            System.out.print('#');
        } else {
            System.out.print('.');
        }

        ++cycleCounter;

        if (cycleCounter % 40 == 0)
            System.out.println();

        if ((cycleCounter - 20) % 40 == 0)
            signalStrengthSum += cycleCounter * registerValue;
    }
}