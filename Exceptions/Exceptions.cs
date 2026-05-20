namespace BiblioSystem.Exceptions;

public class Exceptions(string message) : Exception(message);

public class BusinessException(string message) : Exception(message);

public class UnauthorizedException(string message) : Exception(message);
