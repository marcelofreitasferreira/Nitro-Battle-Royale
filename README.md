# Nitro Battle Royale Mobile

Protótipo original de jogo de tiro em terceira pessoa para Android, feito com Unity.

## Estado
Código inicial. Ainda não há APK, cena Unity configurada nem multiplayer público funcional.

## Unity
Use Unity 6 com suporte Android e instale via Package Manager:
- Netcode for GameObjects
- Unity Transport

## Montagem do protótipo
1. Crie um projeto Unity 3D URP.
2. Crie uma cena com Plane, iluminação e alguns Cubes como obstáculos.
3. Adicione um GameObject NetworkManager com componentes NetworkManager e UnityTransport.
4. Crie uma Capsule com NetworkObject, CharacterController, NetworkTransform e os scripts PlayerMotor e PlayerHealth.
5. Registre a Capsule como Player Prefab no NetworkManager.
6. Crie um Canvas com botões para Host e Client e vincule o script NetworkMenu.
7. Teste duas instâncias na mesma rede. O script PlayerMotor usa WASD no editor; controles touch ainda serão adicionados.

## Limitações
Movimentação inicial sem predição ou validação de servidor; combate, HUD, controles touch, Relay/Lobby e matchmaking são etapas futuras. Para internet pública será necessário Relay/Lobby ou infraestrutura equivalente.
