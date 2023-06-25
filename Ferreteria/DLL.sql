-- TABLES

CREATE TABLE asientos_p5 (
    id NUMBER PRIMARY KEY,
    descripcion VARCHAR2(255),
    asientoFecha DATE,
    codigo VARCHAR2(4),
    nombre VARCHAR2(255),
    cuenta VARCHAR2(255),
    movimiento CHAR(2),
    monto NUMBER
);