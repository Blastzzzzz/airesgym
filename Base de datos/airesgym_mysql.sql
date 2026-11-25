-- =====================================================================
-- AIRESGYM - Script de base de datos para MySQL / MariaDB (phpMyAdmin)
-- Pensado para una base de datos NUEVA y VACIA.
-- Como usarlo: phpMyAdmin -> pestaña SQL -> pegar todo -> Continuar
-- =====================================================================

SET NAMES utf8mb4;

CREATE DATABASE IF NOT EXISTS airesgymdb
    CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE airesgymdb;

-- ---------- SOCIO ----------
CREATE TABLE socio (
    id_socio       INT AUTO_INCREMENT PRIMARY KEY,
    rut            VARCHAR(12)  NOT NULL UNIQUE,
    nombre         VARCHAR(100) NOT NULL,pwd
    telefono       VARCHAR(20)  NOT NULL,
    estado         VARCHAR(20)  NOT NULL DEFAULT 'Activo',
    fecha_registro DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CHECK (estado IN ('Activo', 'Inactivo'))
);

-- ---------- PLAN (tabla madre) y sus dos tablas hijas ----------
CREATE TABLE plan (
    id_plan     INT AUTO_INCREMENT PRIMARY KEY,
    nombre      VARCHAR(50)   NOT NULL UNIQUE,
    precio_base DECIMAL(10,2) NOT NULL,
    CHECK (precio_base > 0)
);

CREATE TABLE plan_diario (
    id_plan       INT PRIMARY KEY,
    horas_validez INT NOT NULL,
    CHECK (horas_validez > 0),
    FOREIGN KEY (id_plan) REFERENCES plan(id_plan)
);

CREATE TABLE plan_por_mes (
    id_plan        INT PRIMARY KEY,
    cantidad_meses INT NOT NULL,
    CHECK (cantidad_meses > 0),
    FOREIGN KEY (id_plan) REFERENCES plan(id_plan)
);

-- ---------- EMPLEADO ----------
CREATE TABLE empleado (
    id_empleado INT AUTO_INCREMENT PRIMARY KEY,
    rut         VARCHAR(12)  NOT NULL UNIQUE,
    nombre      VARCHAR(100) NOT NULL,
    telefono    VARCHAR(20)  NOT NULL,
    es_dueno    BOOLEAN      NOT NULL DEFAULT FALSE,
    activo      BOOLEAN      NOT NULL DEFAULT TRUE
);

-- ---------- MEMBRESIA ----------
CREATE TABLE membresia (
    id_membresia  INT AUTO_INCREMENT PRIMARY KEY,
    id_socio      INT           NOT NULL,
    id_plan       INT           NOT NULL,
    id_empleado   INT           NOT NULL,
    fecha_inicio  DATETIME      NOT NULL,
    fecha_termino DATETIME      NOT NULL,
    monto_pagado  DECIMAL(10,2) NOT NULL,
    metodo_pago   VARCHAR(30)   NOT NULL,
    CHECK (monto_pagado > 0),
    CHECK (fecha_termino > fecha_inicio),
    CHECK (metodo_pago IN ('Efectivo', 'Debito', 'Credito', 'Transferencia')),
    FOREIGN KEY (id_socio)    REFERENCES socio(id_socio),
    FOREIGN KEY (id_plan)     REFERENCES plan(id_plan),
    FOREIGN KEY (id_empleado) REFERENCES empleado(id_empleado)
);

-- =====================================================================
-- DATOS DE PRUEBA
-- =====================================================================

-- Planes reales del gimnasio
INSERT INTO plan (nombre, precio_base) VALUES ('Pase diario', 3000);
INSERT INTO plan_diario (id_plan, horas_validez) VALUES (LAST_INSERT_ID(), 24);

INSERT INTO plan (nombre, precio_base) VALUES ('Mensual adulto mayor', 16990);
INSERT INTO plan_por_mes (id_plan, cantidad_meses) VALUES (LAST_INSERT_ID(), 1);

INSERT INTO plan (nombre, precio_base) VALUES ('Mensual estudiante', 17990);
INSERT INTO plan_por_mes (id_plan, cantidad_meses) VALUES (LAST_INSERT_ID(), 1);

INSERT INTO plan (nombre, precio_base) VALUES ('Mensual general', 20000);
INSERT INTO plan_por_mes (id_plan, cantidad_meses) VALUES (LAST_INSERT_ID(), 1);

INSERT INTO plan (nombre, precio_base) VALUES ('Trimestral estudiante', 44990);
INSERT INTO plan_por_mes (id_plan, cantidad_meses) VALUES (LAST_INSERT_ID(), 3);

INSERT INTO plan (nombre, precio_base) VALUES ('Trimestral general', 55000);
INSERT INTO plan_por_mes (id_plan, cantidad_meses) VALUES (LAST_INSERT_ID(), 3);

INSERT INTO plan (nombre, precio_base) VALUES ('Semestral estudiante', 75990);
INSERT INTO plan_por_mes (id_plan, cantidad_meses) VALUES (LAST_INSERT_ID(), 6);

INSERT INTO plan (nombre, precio_base) VALUES ('Semestral general', 90000);
INSERT INTO plan_por_mes (id_plan, cantidad_meses) VALUES (LAST_INSERT_ID(), 6);

INSERT INTO plan (nombre, precio_base) VALUES ('Anual estudiante', 140990);
INSERT INTO plan_por_mes (id_plan, cantidad_meses) VALUES (LAST_INSERT_ID(), 12);

INSERT INTO plan (nombre, precio_base) VALUES ('Anual general', 170000);
INSERT INTO plan_por_mes (id_plan, cantidad_meses) VALUES (LAST_INSERT_ID(), 12);

-- Empleados de prueba (el primero es el dueño). RUT con digito verificador valido.
INSERT INTO empleado (rut, nombre, telefono, es_dueno, activo) VALUES
('11111111-1', 'Carlos Dueño Vega',   '+56911111111', TRUE,  TRUE),
('22222222-2', 'Andrés Pérez Lagos',  '+56922222222', FALSE, TRUE),
('33333333-3', 'Luis Soto Rivera',    '+56933333333', FALSE, TRUE);

-- Socios de prueba (nombres inventados, RUT con digito verificador valido)
INSERT INTO socio (rut, nombre, telefono) VALUES
('19876543-0', 'Camila Fuentes Soto',     '+56912345678'),
('20123456-5', 'Matías Rojas Pizarro',    '+56923456789'),
('18456123-9', 'Javiera Muñoz Araya',     '+56934567890'),
('17654321-3', 'Benjamín Contreras Vera', '+56945678901'),
('21345678-4', 'Sofía Espinoza Leiva',    '+56956789012'),
('16789234-5', 'Ignacio Tapia Henríquez', '+56967890123');

-- Membresias de prueba (fechas relativas a HOY):
-- 1) POR VENCER: termina en 3 dias
INSERT INTO membresia (id_socio, id_plan, id_empleado, fecha_inicio, fecha_termino, monto_pagado, metodo_pago)
VALUES (
    (SELECT id_socio FROM socio WHERE rut = '19876543-0'),
    (SELECT id_plan FROM plan WHERE nombre = 'Mensual general'),
    (SELECT id_empleado FROM empleado WHERE rut = '22222222-2'),
    DATE_SUB(NOW(), INTERVAL 27 DAY), DATE_ADD(NOW(), INTERVAL 3 DAY), 20000, 'Debito');

-- 2) VENCIDA: termino hace 5 dias
INSERT INTO membresia (id_socio, id_plan, id_empleado, fecha_inicio, fecha_termino, monto_pagado, metodo_pago)
VALUES (
    (SELECT id_socio FROM socio WHERE rut = '20123456-5'),
    (SELECT id_plan FROM plan WHERE nombre = 'Mensual general'),
    (SELECT id_empleado FROM empleado WHERE rut = '22222222-2'),
    DATE_SUB(NOW(), INTERVAL 35 DAY), DATE_SUB(NOW(), INTERVAL 5 DAY), 20000, 'Efectivo');

-- 3) VIGENTE, vendida este mes (sirve para probar el reporte de ingresos)
INSERT INTO membresia (id_socio, id_plan, id_empleado, fecha_inicio, fecha_termino, monto_pagado, metodo_pago)
VALUES (
    (SELECT id_socio FROM socio WHERE rut = '18456123-9'),
    (SELECT id_plan FROM plan WHERE nombre = 'Trimestral estudiante'),
    (SELECT id_empleado FROM empleado WHERE rut = '33333333-3'),
    NOW(), DATE_ADD(NOW(), INTERVAL 3 MONTH), 44990, 'Transferencia');

SELECT 'Base de datos airesgymdb creada correctamente' AS resultado;
