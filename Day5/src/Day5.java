import java.io.*;
import java.util.Arrays;
import java.util.Stack;
import java.util.regex.Pattern;
import java.util.stream.Collector;

public class Day5 {
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

    private static void solve() throws IOException {
        var input = new File("inputs/day5.txt");
        var br = new BufferedReader(new FileReader(input));

        Stack<Character>[] stacks = parseStacks(br);
        br.readLine();

        applyMoves(stacks, br);
    }

    private static Stack<Character>[] parseStacks(BufferedReader br) throws IOException {
        String line;

        Stack<String> crateLines = new Stack<>();

        // First save all lines with crates for later
        while ((line = br.readLine()) != null && line.charAt(1) != '1') {
            crateLines.push(line);
        }

        // Then parse how many stacks there are
        int stackCount = (line.length() + 2) / 4;

        Stack<Character>[] stacks = new Stack[stackCount];
        for (int i = 0; i < stacks.length; i++) {
            stacks[i] = new Stack<>();
        }

        // Now for each line of crates, add in the respecting stack
        while (!crateLines.isEmpty()) {
            line = crateLines.pop();

            int stackIndex = 0;
            // Each stack is 3 chars wide, 1 char space in between
            for (int j = 1; j < line.length(); j += 4, ++stackIndex) {
                char content = line.charAt(j);
                if (content != ' ')
                    stacks[stackIndex].push(line.charAt(j));
            }
        }

        return stacks;
    }

    private static void applyMoves(Stack<Character>[] stacksPartOne, BufferedReader br) throws IOException {
        Stack<Character>[] stacksPartTwo = new Stack[stacksPartOne.length];
        for (int i = 0; i < stacksPartOne.length; i++) {
            stacksPartTwo[i] = (Stack) stacksPartOne[i].clone();
        }

        var tempStack = new Stack<Character>();

        var pattern = Pattern.compile("[0-9]+");

        String line;

        while ((line = br.readLine()) != null) {
            int[] move = pattern.matcher(line).results()
                    .mapToInt(r -> Integer.parseInt(r.group()))
                    .toArray();

            for (int i = 0; i < move[0]; i++) {
                stacksPartOne[move[2] - 1].push(stacksPartOne[move[1] - 1].pop());

                tempStack.push(stacksPartTwo[move[1] - 1].pop());
            }

            for (int i = 0; i < move[0]; i++) {
                stacksPartTwo[move[2] - 1].push(tempStack.pop());
            }
        }

        printResults(stacksPartOne);
        printResults(stacksPartTwo);
    }

    private static void printResults(Stack<Character>[] stacks) {
        System.out.println(Arrays.stream(stacks)
                .map(Stack::pop)
                .collect(Collector.of(
                        StringBuilder::new,
                        StringBuilder::append,
                        StringBuilder::append,
                        StringBuilder::toString))
        );
    }
}