CREATE DATABASE biblio_system;
USE biblio_system;

CREATE TABLE categorias (
    id INT AUTO_INCREMENT PRIMARY KEY,
    descricao VARCHAR(100) NOT NULL
);

CREATE TABLE livros (
    id INT AUTO_INCREMENT PRIMARY KEY,
    titulo VARCHAR(150) NOT NULL,
    autor VARCHAR(100) NOT NULL,
    ano INT NOT NULL,
    categoria_id INT,
    FOREIGN KEY (categoria_id) REFERENCES categorias(id)
);

CREATE TABLE exemplares (
    id INT AUTO_INCREMENT PRIMARY KEY,
    livro_id INT NOT NULL,
    status VARCHAR(50) NOT NULL DEFAULT 'disponivel',
    FOREIGN KEY (livro_id) REFERENCES livros(id)
);

CREATE TABLE membros (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL
);

CREATE TABLE emprestimos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    exemplar_id INT NOT NULL,
    membro_id INT NOT NULL,
    data_emprestimo DATE NOT NULL,
    data_devolucao_prevista DATE NOT NULL,
    data_devolucao DATE NULL,
    FOREIGN KEY (exemplar_id) REFERENCES exemplares(id),
    FOREIGN KEY (membro_id) REFERENCES membros(id)
);

CREATE TABLE reservas (
    id INT AUTO_INCREMENT PRIMARY KEY,
    livro_id INT NOT NULL,
    membro_id INT NOT NULL,
    data_reserva DATE NOT NULL,
    FOREIGN KEY (livro_id) REFERENCES livros(id),
    FOREIGN KEY (membro_id) REFERENCES membros(id)
);

INSERT INTO categorias (descricao) VALUES 
('Romance'),
('Tecnologia'),
('História');

INSERT INTO livros (titulo, autor, ano, categoria_id) VALUES
('Dom Casmurro', 'Machado de Assis', 1899, 1),
('Clean Code', 'Robert C. Martin', 2008, 2),
('Sapiens', 'Yuval Noah Harari', 2011, 3);

INSERT INTO exemplares (livro_id, status) VALUES
(1, 'disponivel'),
(1, 'emprestado'),
(2, 'disponivel'),
(3, 'disponivel'),
(3, 'disponivel');

INSERT INTO membros (nome, email) VALUES
('João Silva', 'joao@email.com'),
('Maria Souza', 'maria@email.com');

INSERT INTO emprestimos (exemplar_id, membro_id, data_emprestimo, data_devolucao_prevista, data_devolucao) VALUES
(2, 1, '2026-04-10', '2026-04-20', NULL);

INSERT INTO reservas (livro_id, membro_id, data_reserva) VALUES
(1, 2, '2026-04-14');