import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import java.util.*;
import java.util.regex.Pattern;

public class Day11 {
    public static void main(String[] args) throws IOException {
        long elapsedTimeSum = 0;
        for (int i = 0; i < 1000; i++) {
            long startTime = System.nanoTime();

            var monkeysPartOne = parseInput();
            var monkeysPartTwo = new ArrayList<Monkey>();
            for (var monkey : monkeysPartOne) {
                monkeysPartTwo.add(monkey.copy());
            }

            calculatePartOne(monkeysPartOne);
            calculatePartTwo(monkeysPartTwo);

            long elapsedTime = System.nanoTime() - startTime;
            elapsedTimeSum += elapsedTime;
        }
        System.out.println("average execution time (ns): " + elapsedTimeSum / 1000);
    }

    private static List<Monkey> parseInput() throws IOException {
        var input = new java.io.File("inputs/day11.txt");
        var br = new BufferedReader(new FileReader(input));

        String line;
        var itemPattern = Pattern.compile("[0-9]+");

        var monkeys = new ArrayList<Monkey>();

        while ((line = br.readLine()) != null) {
            var monkey = new Monkey();
            monkeys.add(monkey);

            // First line contains monkey id;
            monkey.id = Integer.parseInt(line.substring(7, line.length() - 1));

            // Second line contains items
            line = br.readLine();
            monkey.items.addAll(
                    itemPattern.matcher(line).results()
                            .mapToLong(r -> Integer.parseInt(r.group()))
                            .boxed()
                            .toList()
            );

            // Third line contains operation
            line = br.readLine();
            monkey.operator = line.charAt(23);
            try {
                monkey.value = Integer.parseInt(line.substring(25));
            } catch (NumberFormatException ignored) {
                monkey.value = 0;
            }


            // Fourth line contains test
            line = br.readLine();
            monkey.testFactor = Integer.parseInt(line.substring(21));

            // Fifth line contains true target
            line = br.readLine();
            monkey.trueTarget = Integer.parseInt(line.substring(29));

            // Sixth line contains false target
            line = br.readLine();
            monkey.falseTarget = Integer.parseInt(line.substring(30));

            // Empty line separating monkeys
            br.readLine();
        }

        return monkeys;
    }

    private static void calculatePartOne(List<Monkey> monkeys) {
        for (int round = 0; round < 20; round++) {
            for (var monkey : monkeys) {
                for (var item : monkey.items) {
                    if (monkey.operator == '+')
                        item += monkey.value;
                    else if (monkey.operator == '*')
                        item *= monkey.value != 0 ? monkey.value : item;

                    item /= 3;

                    if (item % monkey.testFactor == 0)
                        monkeys.get(monkey.trueTarget).items.add(item);
                    else
                        monkeys.get(monkey.falseTarget).items.add(item);
                }
                monkey.inspectionCount += monkey.items.size();
                monkey.items.clear();
            }
        }

        System.out.println(
                monkeys.stream()
                        .map(m -> m.inspectionCount)
                        .sorted(Collections.reverseOrder())
                        .limit(2)
                        .reduce(Math::multiplyExact).orElse(0)
        );
    }

    private static void calculatePartTwo(List<Monkey> monkeys) {
        // Calculate LCM
        var lcm = 1;
        for (var monkey : monkeys) {
            lcm *= monkey.testFactor;
        }

        for (int round = 0; round < 10000; round++) {
            for (var monkey : monkeys) {
                for (var item : monkey.items) {
                    if (monkey.operator == '+')
                        item += monkey.value;
                    else if (monkey.operator == '*')
                        item *= monkey.value != 0 ? monkey.value : item;

                    // Use LCM to keep values from becoming insanely large
                    if (item > lcm) {
                        item %= lcm;
                        if (item < 0)
                            item += lcm;
                    }

                    if (item % monkey.testFactor == 0)
                        monkeys.get(monkey.trueTarget).items.add(item);
                    else
                        monkeys.get(monkey.falseTarget).items.add(item);
                }
                monkey.inspectionCount += monkey.items.size();
                monkey.items.clear();
            }
        }

        System.out.println(
                monkeys.stream()
                        .map(m -> (long) m.inspectionCount)
                        .sorted(Collections.reverseOrder())
                        .limit(2)
                        .reduce(Math::multiplyExact).orElse(0L)
        );
    }
}

class Monkey {
    int id;
    List<Long> items = new LinkedList<>();
    char operator;
    int value;
    int testFactor;
    int trueTarget;
    int falseTarget;
    int inspectionCount;

    Monkey copy() {
        var monkey = new Monkey();

        monkey.id = id;
        monkey.operator = operator;
        monkey.value = value;
        monkey.testFactor = testFactor;
        monkey.trueTarget = trueTarget;
        monkey.falseTarget = falseTarget;
        monkey.items.addAll(items);

        return monkey;
    }
}