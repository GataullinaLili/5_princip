data class Patient(
    val fullName: String,
    val age: Int,
    val diagnosis: String,
    val days: Int
)
fun processPatients(patients: List<Patient>) {
    println("5 принципов функционального программирования (Kotlin)")

    // 1) Функции как аргументы
    fun filterPatients(predicate: (Patient) -> Boolean): List<Patient> {
        return patients.filter(predicate)
    }

    val longStayPatients = filterPatients { it.age > 60 }
    println("\nПациенты старше 60 лет:")
    longStayPatients.forEach { println("${it.fullName}") }

    // 2) Функции как возвращаемое значение
    fun getDiagnosisFilter(diagnosis: String): (Patient) -> Boolean {
        return { it.diagnosis.equals(diagnosis, ignoreCase = true) }
    }

    val pneumoniaPatients = patients.filter(getDiagnosisFilter("J15.9 Бактериальная пневмония неуточненная"))
    println("\nПациенты с диагнозом J15.9:")
    pneumoniaPatients.forEach { println("${it.fullName} — ${it.days} койко-дней") }

    // 3) Композиция функций
    fun composeFilterAndMap(
        filter: (Patient) -> Boolean,
        map: (Patient) -> String
    ): (List<Patient>) -> List<String> {
        return { list ->
            list.filter(filter).map(map)
        }
    }

    val gripPatients = composeFilterAndMap(
        filter = getDiagnosisFilter("J11.0 Грипп с пневмонией, вирус не идентифицирован"),
        map = { "${it.fullName} — ${it.days} койко-дней" }
    )

    println("\nКомпозиция: Пациенты с диагнозом J11.0:")
    gripPatients(patients).forEach { println(it) }

    // 4) Каррирование
    fun curryFilter(minBedDays: Int): (Patient) -> Boolean {
        return { it.days >= minBedDays }
    }

    fun curryMap(): (Patient) -> String {
        return { "Пациент: ${it.fullName}, Койко-дней: ${it.days}" }
    }

    val curriedResult = patients
        .filter(curryFilter(5))
        .map(curryMap())

    println("\nРезультат каррирования (койко-дней >= 5):")
    curriedResult.forEach { println(it) }

    // 5) Встроенные функции высшего порядка
    val totalBedDays = patients.fold(0) { acc, patient -> acc + patient.days }
    val BedDays = totalBedDays.toDouble() / patients.size

    println("\nСреднее количество койко-дней: %.1f".format(BedDays))
}
fun main() {
    val patients = listOf(
        Patient("Петров Иван Иванович", 68, "J11.0 Грипп с пневмонией, вирус не идентифицирован", 5),
        Patient("Смирнова Мария Петровна", 45, "J15.9 Бактериальная пневмония неуточненная", 7),
        Patient("Иванов Алексей Андреевич", 34, "J15.9 Бактериальная пневмония неуточненная", 5),
        Patient("Павлова Анна Львовна", 52, "J12.8 Другая вирусная пневмония", 4),
        Patient("Васильев Николай Иванович", 60, "J11.0 Грипп с пневмонией, вирус не идентифицирован", 9)
    )

    processPatients(patients)
}
