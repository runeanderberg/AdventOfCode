import java.io.*;
import java.util.Arrays;
import java.util.Stack;
import java.util.regex.Pattern;
import java.util.stream.Collector;
import java.util.stream.Collectors;

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
        printResults(stacks);
    }

    private static Stack<Character>[] parseStacks(BufferedReader br) throws IOException {
        String line;

        Stack<String> crateLines = new Stack<>();

        // First save all lines with crates for later
        while ((line = br.readLine()) != null && line.trim().charAt(0) == '[') {
            crateLines.push(line);
        }

        // Then parse how many stacks there are
        int stackCount = (int) Pattern.compile("[0-9]+").matcher(line).results().count();

        Stack<Character>[] stacks = new Stack[stackCount];
        for (int i = 0; i < stacks.length; i++) {
            stacks[i] = new Stack<>();
        }

        // Now for each line of crates, add in the respecting stack
        while (!crateLines.isEmpty()) {
            line = crateLines.pop();

            int stackIndex = 0;
            // Each stack is 3 chars wide, 1 char space in between
            for (int j = 1; j < stackCount * 3 + (stackCount - 1); j += 4, ++stackIndex) {
                char content = j < line.length() ? line.charAt(j) : ' ';
                if (content != ' ')
                    stacks[stackIndex].push(line.charAt(j));
            }
        }

        return stacks;
    }

    private static void applyMoves(Stack<Character>[] stacks, BufferedReader br) throws IOException {
        var pattern = Pattern.compile("[0-9]+");

        String line;

        while ((line = br.readLine()) != null) {
            int[] move = pattern.matcher(line).results()
                    .mapToInt(r -> Integer.parseInt(r.group()))
                    .toArray();

            for (int i = 0; i < move[0]; i++) {
                stacks[move[2] - 1].push(stacks[move[1] - 1].pop());
            }
        }
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