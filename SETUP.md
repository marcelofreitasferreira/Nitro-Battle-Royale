# Preparar Unity 6

1. Instale Unity 6 com Android Build Support.
2. Crie um projeto 3D URP chamado NitroBattleRoyale.
3. Copie os arquivos Assets e .github deste repositório para a raiz do projeto Unity.
4. Instale os pacotes Netcode for GameObjects e Unity Transport via Package Manager.
5. Use o menu Nitro Battle Royale > Create Prototype Arena.
6. Confira o prefab e a configuração do NetworkManager no Inspector.
7. Pressione Play e Host para testar no editor; Client usa o endereço padrão do Unity Transport (localhost). Para dois dispositivos, configure IP e porta ou integre Relay.
8. Envie também ProjectSettings e Packages/manifest.json e packages-lock.json para o repositório.
9. Configure uma licença Unity válida nos secrets do GitHub para habilitar o workflow de compilação Android.

A arena é um protótipo. A movimentação atual usa WASD e não é adequada para Android até adicionarmos controles touch. Não há conexão online pública, matchmaking, tiros ou battle royale completo. O NetworkTransform padrão e a movimentação local ainda precisam de arquitetura autoritativa adequada antes do multiplayer de produção.
