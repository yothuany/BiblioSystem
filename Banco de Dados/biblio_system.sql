
CREATE DATABASE IF NOT EXISTS biblio_system;
USE biblio_system;

CREATE TABLE autores (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    biografia TEXT
);

CREATE TABLE categorias (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    descricao TEXT
);

CREATE TABLE livros (
    id INT AUTO_INCREMENT PRIMARY KEY,
    titulo VARCHAR(200) NOT NULL,
    isbn VARCHAR(20) NOT NULL UNIQUE,
    ano_publicacao INT NOT NULL,
    editora VARCHAR(100) NOT NULL
);

CREATE TABLE livro_autor (
    livro_id INT NOT NULL,
    autor_id INT NOT NULL,
    PRIMARY KEY (livro_id, autor_id),
    CONSTRAINT fk_la_livro FOREIGN KEY (livro_id) REFERENCES livros(id) ON DELETE CASCADE,
    CONSTRAINT fk_la_autor FOREIGN KEY (autor_id) REFERENCES autores(id) ON DELETE CASCADE
);

CREATE TABLE livro_categoria (
    livro_id INT NOT NULL,
    categoria_id INT NOT NULL,
    PRIMARY KEY (livro_id, categoria_id),
    CONSTRAINT fk_lc_livro FOREIGN KEY (livro_id) REFERENCES livros(id) ON DELETE CASCADE,
    CONSTRAINT fk_lc_cat FOREIGN KEY (categoria_id) REFERENCES categorias(id) ON DELETE CASCADE
);

CREATE TABLE exemplares (
    id INT AUTO_INCREMENT PRIMARY KEY,
    codigo VARCHAR(50) NOT NULL,
    status VARCHAR(45) NOT NULL DEFAULT 'disponivel', -- disponivel | emprestado | reservado
    livro_id INT NOT NULL,
    CONSTRAINT fk_exemplar_livro FOREIGN KEY (livro_id) REFERENCES livros(id) ON DELETE CASCADE
);

CREATE TABLE membros (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    telefone VARCHAR(20) NOT NULL,
    cpf VARCHAR(14) NOT NULL UNIQUE,
    data_cadastro DATE NOT NULL DEFAULT (CURRENT_DATE)
);

CREATE TABLE usuarios (
    id INT AUTO_INCREMENT PRIMARY KEY,
    email VARCHAR(150) NOT NULL UNIQUE,
    senha VARCHAR(200) NOT NULL, -- hash BCrypt
    membro_id INT NOT NULL UNIQUE,
    CONSTRAINT fk_usuario_membro FOREIGN KEY (membro_id) REFERENCES membros(id) ON DELETE CASCADE
);

CREATE TABLE emprestimos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    data_emprestimo DATE NOT NULL,
    data_devolucao_prevista DATE NOT NULL,
    data_devolucao DATE,
    valor_multa DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    membro_id INT NOT NULL,
    exemplar_id INT NOT NULL,
    CONSTRAINT fk_emp_membro FOREIGN KEY (membro_id) REFERENCES membros(id) ON DELETE RESTRICT,
    CONSTRAINT fk_emp_exemplar FOREIGN KEY (exemplar_id) REFERENCES exemplares(id) ON DELETE RESTRICT
);

CREATE TABLE reservas (
    id INT AUTO_INCREMENT PRIMARY KEY,
    data_reserva DATE NOT NULL DEFAULT (CURRENT_DATE),
    status VARCHAR(45) NOT NULL DEFAULT 'pendente', -- pendente | atendida | cancelada
    membro_id INT NOT NULL,
    livro_id INT NOT NULL,
    CONSTRAINT fk_res_membro FOREIGN KEY (membro_id) REFERENCES membros(id) ON DELETE RESTRICT,
    CONSTRAINT fk_res_livro FOREIGN KEY (livro_id) REFERENCES livros(id) ON DELETE RESTRICT
);

INSERT INTO autores (nome, biografia) VALUES
('Machado de Assis', 'Escritor brasileiro, considerado o maior nome da literatura nacional.'),
('Robert C. Martin', 'Engenheiro de software e autor de livros sobre boas práticas de programação.'),
('Yuval Noah Harari', 'Historiador e professor israelense, autor de obras sobre a história da humanidade.');

INSERT INTO categorias (nome, descricao) VALUES
('Romance', 'Obras de ficção com foco em relacionamentos e sentimentos.'),
('Tecnologia', 'Livros sobre programação, sistemas e engenharia de software.'),
('História', 'Obras sobre fatos e períodos históricos da humanidade.');

INSERT INTO livros (titulo, isbn, ano_publicacao, editora) VALUES
('Dom Casmurro', '978-8572326094', 1899, 'Ática'),
('Clean Code', '978-0132350884', 2008, 'Prentice Hall'),
('Sapiens', '978-8535922851', 2011, 'Companhia das Letras');

INSERT INTO livro_autor (livro_id, autor_id) VALUES
(1, 1),
(2, 2),
(3, 3);

INSERT INTO livro_categoria (livro_id, categoria_id) VALUES
(1, 1),
(2, 2),
(3, 3);

INSERT INTO exemplares (codigo, status, livro_id) VALUES
('EX-001', 'disponivel', 1),
('EX-002', 'emprestado', 1),
('EX-003', 'disponivel', 2),
('EX-004', 'disponivel', 3),
('EX-005', 'disponivel', 3);

INSERT INTO membros (nome, email, telefone, cpf) VALUES
('João Silva', 'joao@email.com', '(69) 99999-0001', '111.111.111-11'),
('Maria Souza', 'maria@email.com', '(69) 99999-0002', '222.222.222-22');

INSERT INTO usuarios (email, senha, membro_id) VALUES
('joao@email.com', '$2a$12$examplehashjoao', 1),
('maria@email.com', '$2a$12$examplehashmaria', 2);

INSERT INTO emprestimos (data_emprestimo, data_devolucao_prevista, data_devolucao, valor_multa, membro_id, exemplar_id) VALUES
('2026-04-10', '2026-04-20', NULL, 0.00, 1, 2);

INSERT INTO reservas (data_reserva, status, membro_id, livro_id) VALUES
('2026-04-14', 'pendente', 2, 1);