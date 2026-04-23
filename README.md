# PruebaTecnicaArquitectoLiga

Se identifican siete bounded contexts principales: Squad Management, Transfer Management, Financial Management, Competition Management, Performance Analytics, Facilities Management y Fan Engagement. Esta separación permite aislar reglas de negocio, escalar de forma independiente las capacidades más exigentes y reducir el acoplamiento entre dominios con distintos ritmos de cambio.


La propuesta adopta una arquitectura híbrida. Los dominios con alta variabilidad funcional y necesidades de escalado independiente, como eventos en tiempo real, analítica o portal de fans, encajan mejor en servicios desacoplados. En cambio, dominios altamente transaccionales y con fuerte coherencia de negocio, como fichajes, plantillas y finanzas, pueden partir de un diseño modular bien delimitado, minimizando complejidad distribuida innecesaria y facilitando una evolución progresiva.
