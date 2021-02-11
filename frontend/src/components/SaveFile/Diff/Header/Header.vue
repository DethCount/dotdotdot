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
                        <td>From file</td>
                        <td>{{ filename1 }}</td>
                    </tr>
                    <tr>
                        <td>To file</td>
                        <td>{{ filename2 }}</td>
                    </tr>
                    <tr v-bind:key="key" v-for="(value, key) in header">
                        <td>{{ key }}</td>
                        <td v-if="value.status == 'modified'">
                            <span class="deleted">{{ value.from }}</span>
                            <span class="added">{{ value.to }}</span>
                        </td>
                        <td v-else-if="value.status == 'added'">
                            <span class="added">{{ value.to }}</span>
                        </td>
                        <td v-else-if="value.status == 'deleted'">
                            <span class="deleted">{{ value.from }}</span>
                        </td>
                        <td v-else>{{ value }}</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="col-12" v-else>
            <Spinner></Spinner>
        </div>
    </div>
</template>

<script src="@/components/SaveFile/Diff/Header/Header.js"></script>

<style scoped lang="sass" src="@/components/SaveFile/Diff/Header/Header.sass"></style>
