<template>
    <div class="row">
        <div class="col-12" v-if="loaded">
            <table class="table table-striped">
                <thead>
                    <th>Field</th>
                    <th>Value</th>
                </thead>
                <tbody>
                    <tr>
                        <td>Filename</td>
                        <td>{{ filename }}</td>
                    </tr>
                    <tr v-bind:key="key" v-for="(value, key) in header">
                        <td>{{ key }}</td>
                        <td>{{ value }}</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="col-12" v-else>
            <Spinner></Spinner>
        </div>
    </div>
</template>

<script src="@/components/SaveFile/Header/Header.js"></script>

<style scoped lang="sass" src="@/components/SaveFile/Header/Header.sass"></style>
