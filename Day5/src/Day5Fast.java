import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.io.IOException;
import java.util.Arrays;
import java.util.Stack;

public class Day5Fast {
    public static void main(String[] args) throws IOException {
        long elapsedTimeSum = 0;
        for (int i = 0; i < 1000; i++) {
            long startTime = System.nanoTime();
            solve();
            long elapsedTime = System.nanoTime() - startTime;
            elapsedTimeSum += elapsedTime;
        }
        System.out.println("average execution time (ns): " + elapsedTimeSum/1000);
    }

    static class Wrapper {
        StacksWrapper partOne;
        StacksWrapper partTwo;

        Wrapper(StacksWrapper partOne, StacksWrapper partTwo) {
            this.partOne = partOne;
            this.partTwo = partTwo;
        }
    }

    static class StacksWrapper {
        char[][] stack;
        int[] count;

        StacksWrapper(char[][] stack, int[] count) {
            this.stack = stack;
            this.count = count;
        }

        StacksWrapper copy() {
            return new StacksWrapper(
                    Arrays.stream(stack)
                            .map(char[]::clone)
                            .toArray(char[][]::new),
                    count.clone());
        }
    }

    private static void solve() throws IOException {
        var input = new File("inputs/day5.txt");
        var br = new BufferedReader(new FileReader(input));

        var sw = parseStacks(br);
        var w = new Wrapper(sw, sw.copy());
        br.readLine();

        applyMoves(w, br);
        printResults(w);
    }

    private static StacksWrapper parseStacks(BufferedReader br) throws IOException {
        String line;

        Stack<String> crateLines = new Stack<>();

        // First save all lines with crates for later
        while ((line = br.readLine()) != null && line.charAt(1) != '1') {
            crateLines.push(line);
        }

        // Then parse how many stacks there are
        int stackCount = (line.length() + 2) / 4;

        // Allocate space for crates, worse case scenario all crates are in one stack
        char[][] stacks = new char[stackCount][crateLines.size() * stackCount];
        var indices = new int[stackCount];

        // Now for each line of crates, add in the respecting stack
        while (!crateLines.isEmpty()) {
            line = crateLines.pop();

            int stackIndex = 0;
            // Each stack is 3 chars wide, 1 char space in between
            for (int j = 1; j < line.length(); j += 4, ++stackIndex) {
                char content = line.charAt(j);
                if (content != ' ') {
                    stacks[stackIndex][indices[stackIndex]++] = line.charAt(j);
                }
            }
        }

        return new StacksWrapper(stacks, indices);
    }

    private static void applyMoves(Wrapper w, BufferedReader br) throws IOException {
        var stackOne = w.partOne.stack;
        var countOne = w.partOne.count;
        var stackTwo = w.partTwo.stack;
        var countTwo = w.partTwo.count;

        String line;

        while ((line = br.readLine()) != null) {
            String[] move = line.split(" ");

            int count = Integer.parseInt(move[1]);
            int fromCol = Integer.parseInt(move[3]) - 1;
            int toCol = Integer.parseInt(move[5]) - 1;

            for (int i = 0; i < count; i++) {
                stackOne[toCol][countOne[toCol]++] = stackOne[fromCol][--countOne[fromCol]];
                stackTwo[toCol][countTwo[toCol]++] = stackTwo[fromCol][countTwo[fromCol]++ - count];
            }

            // Fix broken count, TODO: figure out better way to do so count don't break
            countTwo[fromCol] -= count * 2;
        }
    }

    private static void printResults(Wrapper w) {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < w.partOne.stack.length; i++) {
            sb.append(w.partOne.stack[i][w.partOne.count[i] - 1]);
        }
        sb.append('\n');
        for (int i = 0; i < w.partTwo.stack.length; i++) {
            sb.append(w.partTwo.stack[i][w.partTwo.count[i] - 1]);
        }
        
        System.out.println(sb);
    }
}