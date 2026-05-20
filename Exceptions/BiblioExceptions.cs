namespace BiblioSystem.Exceptions;

public class NotFoundException(string message) : Exception(message);

public class BusinessException(string message) : Exception(message);

public class UnauthorizedException(string message) : Exception(message);
