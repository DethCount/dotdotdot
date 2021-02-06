<template>
    <div class="row">
        <h2>Header</h2>
        <div class="col-12" v-if="loaded">
            <table class="table table-striped">
                <thead>
                    <th>Field</th>
                    <th>Value</th>
                </thead>
                <tbody v-if="loaded">
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
            <h3>Loading...</h3>
        </div>
    </div>
</template>

<script src="@/components/SaveFile/Header/Header.js"></script>

<style scoped lang="scss" src="@/components/SaveFile/Header/Header.scss"></style>
