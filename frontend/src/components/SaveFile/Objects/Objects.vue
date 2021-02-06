<template>
    <div class="row">
        <h2>Objects</h2>
        <div class="col-12" v-if="objectsLoaded">
            <table class="table table-striped">
                <thead>
                    <th>rootObject</th>
                    <th>instanceName</th>
                    <th>type</th>
                    <th>typePath</th>
                    <th>parentEntityName</th>
                    <th>needTransform</th>
                    <th>rotation</th>
                    <th>position</th>
                    <th>scale</th>
                    <th>wasPlacedInLevel</th>
                    <th>properties</th>
                </thead>
                <tbody>
                    <tr v-bind:key="obj.id" v-for="obj in objects">
                        <td>{{ obj.id.rootObject }}</td>
                        <td>{{ obj.id.instanceName }}</td>
                        <td>{{ obj.value.type }}</td>
                        <td>{{ obj.typePath }}</td>
                        <td>{{ obj.value.parentEntityName }}</td>
                        <td>{{ obj.value.needTransform }}</td>
                        <td>{{ obj.value.rotation }}</td>
                        <td>{{ obj.value.position }}</td>
                        <td>{{ obj.value.scale }}</td>
                        <td>{{ obj.value.wasPlacedInLevel }}</td>
                        <td></td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="col-12" v-else>
            <h3>Loading...</h3>
        </div>
    </div>
</template>

<script src="@/components/SaveFile/Objects/Objects.js"></script>

<style lang="scss" scoped src="@/components/SaveFile/Objects/Objects.scss"></style>
